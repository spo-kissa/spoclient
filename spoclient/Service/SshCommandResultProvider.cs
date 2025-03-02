using spoclient.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient.Service
{
    /// <summary>
    ///     SSHコマンドの実行結果を提供するクラス
    /// </summary>
    public class SshCommandResultProvider
    {
        private string commandText = string.Empty;
        private string resultText = string.Empty;
        private int exitCode = -1;
        private SshCommandResult result = new(string.Empty, string.Empty, -1);
        private SshServiceState state;
        private readonly object lockObject = new();
        private event EventHandler<SshCommandResult>? StateChanged;


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SshCommandResultProvider()
        {
            state = SshServiceState.Idle;
        }


        /// <summary>
        ///     コマンドテキストを設定する
        /// </summary>
        /// <param name="value">コマンドテキスト</param>
        /// <returns>自身のインスタンスを返します</returns>
        public SshCommandResultProvider SetCommandText(string value)
        {
            lock (lockObject)
            {
                commandText = value;
            }

            return this;
        }


        /// <summary>
        ///     結果テキストを設定する
        /// </summary>
        /// <param name="value">結果テキスト</param>
        /// <returns>自身のインスタンスを返します</returns>
        public SshCommandResultProvider SetResult(string value)
        {
            lock (lockObject)
            {
                resultText = value;
            }

            return this;
        }


        /// <summary>
        ///    終了コードを設定する
        /// </summary>
        /// <param name="value">終了コード</param>
        /// <returns>自身のインスタンスを返します</returns>
        public SshCommandResultProvider SetExitCode(int value)
        {
            lock (lockObject)
            {
                exitCode = value;
            }

            return this;
        }


        /// <summary>
        ///     状態を設定する
        /// </summary>
        /// <param name="newState">状態</param>
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


        /// <summary>
        ///     結果が取得できるまで待機し、取得した結果を返します
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
