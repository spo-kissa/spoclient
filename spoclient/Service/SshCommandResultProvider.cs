using spoclient.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public class SshCommandResultProvider
    {
        private string commandText = string.Empty;
        private string resultText = string.Empty;
        private int exitCode = -1;
        private SshCommandResult result = new(string.Empty, string.Empty, -1);
        private SshServiceState state;
        private readonly object lockObject = new();
        private event EventHandler<SshCommandResult>? StateChanged;


        public SshCommandResultProvider()
        {
            state = SshServiceState.Idle;
        }


        public SshCommandResultProvider SetCommandText(string value)
        {
            lock (lockObject)
            {
                commandText = value;
            }

            return this;
        }


        public SshCommandResultProvider SetResult(string value)
        {
            lock (lockObject)
            {
                resultText = value;
            }

            return this;
        }


        public SshCommandResultProvider SetExitCode(int value)
        {
            lock (lockObject)
            {
                exitCode = value;
            }

            return this;
        }


        public void SetState(SshServiceState newState)
        {
            var newValue = new SshCommandResult(commandText, resultText, exitCode);
            lock (lockObject)
            {
                result = newValue;
                state = newState;
            }
            StateChanged?.Invoke(this, newValue);
        }


        public async Task<SshCommandResult> GetResultWhenCommandExitedAsync(CancellationToken cancellationToken)
        {
            var completionSource = new TaskCompletionSource<SshCommandResult>();

            void Handler(object? sender, SshCommandResult value)
            {
                lock (lockObject)
                {
                    if (state == SshServiceState.Idle)
                    {
                        completionSource.SetResult(result);
                        StateChanged -= Handler;
                    }
                }
            }

            StateChanged += Handler;

            cancellationToken.Register(() =>
            {
                completionSource.TrySetCanceled();
                StateChanged -= Handler;
            });

            return await completionSource.Task;
        }
    }
}
