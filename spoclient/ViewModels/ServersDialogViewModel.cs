using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Avalonia.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using spoclient.Models;

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


        private readonly ObservableCollection<ServerInfo> servers = [];


        /// <summary>
        ///     接続先サーバーリスト(読み取り専用)
        /// </summary>
        public ReadOnlyObservableCollection<ServerInfo> Servers { get; private set; }


        /// <summary>
        ///     選択中接続サーバー
        /// </summary>
        public ServerInfo? SelectedServer { get; private set; }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ServersDialogViewModel()
        {
            this.Servers = new ReadOnlyObservableCollection<ServerInfo>(this.servers);
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
            var raw = "apJqUK57";
            var password = new SecureString();
            foreach (var ch in raw)
            {
                password.AppendChar(ch);
            }

            servers.Add(new ServerInfo("Hyper-V", "172.21.46.241", "daisuke", password, "22"));
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
                    SelectedServer = e.AddedItems[0] as ServerInfo;
                    return;
                }
            }
            SelectedServer = null;
        });
    }
}