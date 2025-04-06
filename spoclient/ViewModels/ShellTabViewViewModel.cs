using static AnsiVtConsole.NetCore.Component.Console.Unicode;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Events;
using spoclient.Events;
using spoclient.Models;
using spoclient.Plugins.Recipe;
using spoclient.Service;
using spoclient.Terminals;
using SpoClient.Plugin.Recipe.V1;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using VtNetCore.Avalonia;
using AnsiVtConsole.NetCore;
using System.Threading.Tasks;
using Microlithix.Text.Ansi;
using Microlithix.Text.Ansi.Element;
using SpoClient.Setting.Models;

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
        public SecureServer? SecureServer { get; private set; }


        /// <summary>
        ///     SSH接続オブジェクト
        /// </summary>
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
        ///     
        /// </summary>
        public ObservableCollection<RecipeViewModel> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Items)));
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


        private ObservableCollection<RecipeViewModel> items { get; set; } = [];


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


        private AnsiStringParser parser = new();


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ShellTabViewViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Items = [];

            // プラグインを初期化
            foreach (var recipe in RecipeV1Loader.Recipes)
            {
                var model = new RecipeViewModel(new RecipeModel(0, recipe));
                model.GetConnection = (() =>
                {
                    return Connection;
                });
                Items.Add(model);
            }
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
        public async void Connect(SecureServer secureServer)
        {
            var cancellationSource = new CancellationTokenSource();

            SecureServer = secureServer;

            Connection = new SshConnection(secureServer);
            Connection.StateChanged += OnSshStateChanged;
            Connection.DataReceived += OnSshDataReceived;


            if (!await Connection.ConnectAsync(cancellationSource.Token))
            {
                return;
            }

            //await Task.Run(async () =>
            //{
            //    var buffer = new char[4096];

            //    while (Connection?.IsConnected ?? false && !cancellationSource.IsCancellationRequested)
            //    {
            //        var received = await console.In.ReadAsync(buffer, 0, buffer.Length);

            //        if (received > 0)
            //        {
            //            var receivedChars = new char[received];
            //            Buffer.BlockCopy(buffer, 0, receivedChars, 0, received);

            //            System.Diagnostics.Debug.WriteLine(new string(receivedChars));
            //        }

            //        await Task.Delay(5);
            //    }

            //}, cancellationSource.Token);

            //SshService = new SshService(serverInfo);
            //SshService.Output += OnSshOutput;
            //SshService.StateChanged += OnSshStateChanged;
            //await SshService.ConnectAsync();
        }


        private void OnSshDataReceived(object? sender, DataReceivedEventArgs e)
        {
            var stream = new AnsiStreamParser();

            var text = Encoding.UTF8.GetString(e.Data);

            var elements = parser.Parse(text);

            foreach (IAnsiStringParserElement element in elements)
            {
                if (element is AnsiPrintableString printable)
                {
                    System.Diagnostics.Debug.Print($"{printable.Text}");
                }
                else if (element is AnsiPrintableChar printableChar)
                {
                    System.Diagnostics.Debug.Print($"{printableChar.Character}");
                }
                else if (element is AnsiControlString controlString)
                {
                    //System.Diagnostics.Debug.Print($"{controlString.Text}");
                }
                else
                {
                    //System.Diagnostics.Debug.Print($"{element.GetType()}");
                }
            }

//            var text = Encoding.UTF8.GetString(e.Data);
//            System.Diagnostics.Debug.WriteLine(text);
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
                    SetHeader(SecureServer!.Name);
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


        public DelegateCommand CheckPackageUpdate => new(() =>
        {
            Connection?.SendCommand("apt list --upgradable");
        });
    }
}