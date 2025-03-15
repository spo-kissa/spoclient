using Avalonia.Controls;
using Prism.Commands;
using Prism.Events;
using spoclient.Events;
using spoclient.Models;
using spoclient.Service;
using spoclient.Terminals;
using System;
using System.ComponentModel;
using System.Threading;
using VtNetCore.Avalonia;

namespace spoclient.ViewModels
{
    /// <summary>
    ///     シェルタブのビューモデル
    /// </summary>
    public class ShellTabViewViewModel : MainTabViewModel
    {
        /// <summary>
        ///     タブを閉じるイベント
        /// </summary>
        public override event RequestCloseEventHandler? RequestClose;


        /// <summary>
        ///     タブのヘッダー
        /// </summary>
        public override string Header { get => header; }


        /// <summary>
        ///     サーバー情報
        /// </summary>
        public SecureServerInfo? ServerInfo { get; private set; }



        public SshConnection? Connection
        {
            get => sshConnection;
            set
            {
                sshConnection = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Connection)));
            }
        }


        /// <summary>
        ///     ターミナル出力
        /// </summary>
        public string TerminalOutput
        {
            get => terminalOutput;
            set
            {
                terminalOutput = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TerminalOutput)));
            }
        }


        /// <summary>
        ///     ターミナルテキスト
        /// </summary>
        public string TerminalText
        {
            get => terminalText;
            set
            {
                terminalText = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TerminalText)));
            }
        }


        /// <summary>
        ///     タブのヘッダー
        /// </summary>
        private string header = string.Empty;


        /// <summary>
        ///     SSHサービス 
        /// </summary>
        private SshService? SshService { get; set; }


        private SshConnection? sshConnection = null;


        public IEventAggregator EventAggregator => eventAggregator;


        /// <summary>
        ///     ターミナル出力
        /// </summary>
        private string terminalOutput = string.Empty;


        /// <summary>
        ///     ターミナルテキスト
        /// </summary>
        private string terminalText = string.Empty;


        private readonly IEventAggregator eventAggregator;



        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ShellTabViewViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }



        public DelegateCommand<SizeChangedEventArgs> SizeChangedCommand => new(e =>
        {
            eventAggregator.GetEvent<ScrollEvent>().Publish();
        });



        /// <summary>
        ///     ターミナルテキストを実行するコマンド
        /// </summary>
        public DelegateCommand ExecuteTerminalTextCommand => new(() =>
        {
            Connection?.SendCommand(TerminalText);
        });


        /// <summary>
        ///     タブを閉じるコマンド
        /// </summary>
        public override DelegateCommand CloseCommand => new(() =>
        {
            RequestClose?.Invoke(this);
        });


        /// <summary>
        ///     タブヘッダーを設定します
        /// </summary>
        /// <param name="header"></param>
        public void SetHeader(string header)
        {
            this.header = header;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Header)));
        }


        /// <summary>
        ///     サーバーに接続します
        /// </summary>
        /// <param name="serverInfo"></param>
        public async void Connect(SecureServerInfo serverInfo)
        {
            var cancellationSource = new CancellationTokenSource();

            ServerInfo = serverInfo;

            Connection = new SshConnection(serverInfo);

            Connection.DataReceived += OnSshDataReceived;
            

            if (!await Connection.ConnectAsync(cancellationSource.Token))
            {
                return;
            }

            //SshService = new SshService(serverInfo);
            //SshService.Output += OnSshOutput;
            //SshService.StateChanged += OnSshStateChanged;
            //await SshService.ConnectAsync();
        }


        private void OnSshDataReceived(object? sender, DataReceivedEventArgs e)
        {
            
        }


        /// <summary>
        ///     サーバーの接続状態が変更されたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSshStateChanged(object? sender, Terminals.SshStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case SshConnectionState.Connecting:
                    SetHeader("接続しています...");
                    break;

                case SshConnectionState.Connected:
                    SetHeader(ServerInfo!.Entry);
                    break;
            }
        }


        /// <summary>
        ///     SSHの出力があったときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSshOutput(object? sender, SshOutputEventArgs e)
        {
            TerminalOutput += e.Output;
            eventAggregator.GetEvent<ScrollEvent>().Publish();
        }
    }
}