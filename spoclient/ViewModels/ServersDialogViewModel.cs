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
        ///     �_�C�A���O�̕����v������C�x���g
        /// </summary>
        public event Action<IDialogResult>? RequestClose;


        /// <summary>
        ///     �_�C�A���O�̃^�C�g��
        /// </summary>
        public string Title => "Server Select";


        private readonly ObservableCollection<ServerInfo> servers = [];


        /// <summary>
        ///     �ڑ���T�[�o�[���X�g(�ǂݎ���p)
        /// </summary>
        public ReadOnlyObservableCollection<ServerInfo> Servers { get; private set; }


        /// <summary>
        ///     �I�𒆐ڑ��T�[�o�[
        /// </summary>
        public ServerInfo? SelectedServer { get; private set; }


        /// <summary>
        ///     �R���X�g���N�^
        /// </summary>
        public ServersDialogViewModel()
        {
            this.Servers = new ReadOnlyObservableCollection<ServerInfo>(this.servers);
        }


        /// <summary>
        ///     �_�C�A���O�����邩
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        
        /// <summary>
        ///     �_�C�A���O�������̏���
        /// </summary>
        public void OnDialogClosed()
        {
        }


        /// <summary>
        ///     �_�C�A���O���J������̏���
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