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

namespace spoclient.ViewModels
{
    /// <summary>
    ///    サーバー選択ダイアログのViewModel
    /// </summary>
    public class ServersDialogViewModel : ViewModelBase, IDialogAware
    {
        /// <summary>
        ///     ダイアログの閉じるを要求するイベント
        /// </summary>
        public event Action<IDialogResult>? RequestClose;


        /// <summary>
        ///    接続先サーバーリスト
        /// </summary>
        private readonly ObservableCollection<SecureServerInfo> servers = [];


        /// <summary>
        ///     接続先サーバーリスト(読み取り専用)
        /// </summary>
        public ReadOnlyObservableCollection<SecureServerInfo> Servers { get; private set; }


        /// <summary>
        ///     選択中接続サーバー
        /// </summary>
        public SecureServerInfo? SelectedServer { get; private set; }


        /// <summary>
        ///    ダイアログサービス
        /// </summary>
        private readonly IDialogService dialogService;


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="dialogService">ダイアログサービス</param>
        public ServersDialogViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            Servers = new ReadOnlyObservableCollection<SecureServerInfo>(this.servers);
            servers.CollectionChanged += (sender, e) => RaisePropertyChanged(nameof(Servers));

            Title = "Select Server";
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

            this.servers.AddRange(servers);
        }


        /// <summary>
        ///     新規サーバー追加コマンド
        /// </summary>
        public DelegateCommand NewServerCommand => new(() =>
        {
            dialogService.ShowDialog<ServerDialog>(null, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>(nameof(SecureServerInfo));
                    if (serverInfo is not null)
                    {
                        var settings = new ServerSettingsRepository("P@assword");
                        settings.AddServer(serverInfo.ToUnsecure());
                        this.servers.Add(serverInfo);
                    }
                }
            });
        });


        /// <summary>
        ///    サーバー編集コマンド
        /// </summary>
        public DelegateCommand EditServerCommand => new(() =>
        {
            if (SelectedServer is null)
            {
                return;
            }

            var parameters = new DialogParameters()
            {
                { nameof(SecureServerInfo), SelectedServer },
            };
            dialogService.ShowDialog<ServerDialog>(parameters, dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>(nameof(SecureServerInfo));

                    var settings = new ServerSettingsRepository("P@assword");
                    settings.UpdateServer(serverInfo!.ToUnsecure(), SelectedServer.ToUnsecure());
                    var index = this.servers.IndexOf(SelectedServer);
                    this.servers[index] = serverInfo!;
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
                var settings = new ServerSettingsRepository("P@assword");

                settings.DeleteServer(SelectedServer.Entry);
                this.servers.Remove(SelectedServer);
            }
        });


        /// <summary>
        ///     サーバー接続コマンド
        /// </summary>
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
        ///     サーバー選択ダブルクリックコマンド
        /// </summary>
        public DelegateCommand<TappedEventArgs> DoubleTappedCommand => new((e) =>
        {
            ConnectCommand.Execute();
        });


        /// <summary>
        ///    キャンセルボタンコマンド
        /// </summary>
        public DelegateCommand CancelCommand => new(() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        });


        /// <summary>
        ///    サーバー選択変更コマンド
        /// </summary>
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