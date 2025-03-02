using System;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public class SshExitCodeProvider
    {
        private int exitCode = -1;
        private SshServiceState state = SshServiceState.Idle;
        private readonly object lockObject = new();
        private event EventHandler<int>? ValueChanged;


        public void SetValue(int newValue, SshServiceState newState)
        {
            lock (lockObject)
            {
                exitCode = newValue;
                state = newState;
            }
            ValueChanged?.Invoke(this, newValue);
        }


        public async Task<int> GetExitCodeWhenCommandExitedAsync(CancellationToken cancellationToken)
        {
            var completionSource = new TaskCompletionSource<int>();

            void Handler(object? sender, int value)
            {
                lock (lockObject)
                {
                    if (state == SshServiceState.Idle)
                    {
                        completionSource.SetResult(exitCode);
                        ValueChanged -= Handler;
                    }
                }
            }

            ValueChanged += Handler;

            cancellationToken.Register(() =>
            {
                completionSource.TrySetCanceled();
                ValueChanged -= Handler;
            });

            return await completionSource.Task;
        }
    }
}
