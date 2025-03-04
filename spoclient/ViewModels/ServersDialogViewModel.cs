using Avalonia.Controls;
using Avalonia.Input;
using MsBox.Avalonia;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using spoclient.Extensions;
using spoclient.Models;
using System;
using System.Collections.ObjectModel;
using System.Security;

namespace spoclient.ViewModels
{
    public class ServersDialogViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        ///     ダイアログの閉じるを要求するイベント
        /// </summary>
        public event Action<IDialogResult>? RequestClose;


        /// <summary>
        ///     ダイアログのタイトル
        /// </summary>
        public string Title => "Server Select";


        private readonly ObservableCollection<SecureServerInfo> servers = [];


        /// <summary>
        ///     接続先サーバーリスト(読み取り専用)
        /// </summary>
        public ReadOnlyObservableCollection<SecureServerInfo> Servers { get; private set; }


        /// <summary>
        ///     選択中接続サーバー
        /// </summary>
        public SecureServerInfo? SelectedServer { get; private set; }



        private readonly IDialogService dialogService;


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ServersDialogViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            this.Servers = new ReadOnlyObservableCollection<SecureServerInfo>(this.servers);
        }


        /// <summary>
        ///     ダイアログを閉じれるか
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        
        /// <summary>
        ///     ダイアログを閉じた後の処理
        /// </summary>
        public void OnDialogClosed()
        {
        }


        /// <summary>
        ///     ダイアログを開いた後の処理
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            var settings = new ServerSettingsRepository("P@assword");
            var servers = settings.GetServers();

            var raw = "apJqUK57";
            var password = new SecureString();
            foreach (var ch in raw)
            {
                password.AppendChar(ch);
            }

            this.servers.Add(new SecureServerInfo("Hyper-V", "172.21.46.241", "daisuke", password, "22"));
            this.servers.Add(new SecureServerInfo("Hyper-V 2", "172.26.74.167", "daisuke", password, "22"));
        }


        public DelegateCommand ConnectCommand => new(() =>
        {
            if (this.SelectedServer is not null)
            {
                var result = new DialogResult(ButtonResult.OK);

                result.Parameters.Add("ServerInfo", this.SelectedServer!);

                RequestClose?.Invoke(result);
            }
        });


        /// <summary>
        ///     新規サーバー追加コマンド
        /// </summary>
        public DelegateCommand NewServerCommand => new(() =>
        {
            dialogService.ShowDialog<ServerDialog>(null, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>("ServerInfo");
                    if (serverInfo is not null)
                    {
                        this.servers.Add(serverInfo);
                    }
                }
            });
        });


        public DelegateCommand EditServerCommand => new(() =>
        {
            if (SelectedServer is null)
            {
                return;
            }

            var parameters = new DialogParameters()
            {
                { "ServerInfo", SelectedServer },
            };
            dialogService.ShowDialog<ServerDialog>(parameters, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>("ServerInfo");
                }
            });
        });


        /// <summary>
        ///     サーバー削除コマンド
        /// </summary>
        public DelegateCommand DeleteServerCommand => new(async() =>
        {
            if (SelectedServer is null)
            {
                return;
            }

            var msgbox = MessageBoxManager.GetMessageBoxStandard(
                "Delete Sever Entry",
                $"Are you sure you want to delete {SelectedServer.Entry}?",
                MsBox.Avalonia.Enums.ButtonEnum.YesNo,
                MsBox.Avalonia.Enums.Icon.Warning,
                WindowStartupLocation.CenterOwner);

            var result = await msgbox.ShowDialogAsync();
            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                this.servers.Remove(SelectedServer);
            }
        });


        public DelegateCommand<TappedEventArgs> DoubleTappedCommand => new((e) =>
        {
            ConnectCommand.Execute();
        });


        public DelegateCommand CancelCommand => new(() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        });


        public DelegateCommand<SelectionChangedEventArgs> SelectionChangedCommand => new((e) =>
        {
            if (e is not null)
            {
                var count = (e.AddedItems.Count - e.RemovedItems.Count);
                if (count == 1)
                {
                    SelectedServer = e.AddedItems[0] as SecureServerInfo;
                    return;
                }
            }
            SelectedServer = null;
        });
    }
}