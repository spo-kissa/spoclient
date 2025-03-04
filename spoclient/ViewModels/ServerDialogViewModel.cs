using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using spoclient.Extensions;
using spoclient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.ViewModels
{
    public class ServerDialogViewModel : BindableBase, IDialogAware
    {
        public event Action<IDialogResult>? RequestClose;

        public string Title => "Server New Entry";


        private SecureServerInfo secureServerInfo = new();


        public string Entry
        {
            get => secureServerInfo.Entry;
            set
            {
                secureServerInfo.Entry = value;
                RaisePropertyChanged();
            }
        }


        public string Server
        {
            get => secureServerInfo.Server;
            set
            {
                secureServerInfo.Server = value;
                RaisePropertyChanged();
            }
        }


        public string Port
        {
            get => secureServerInfo.Port;
            set
            {
                secureServerInfo.Port = value;
                RaisePropertyChanged();
            }
        }


        public string User
        {
            get => secureServerInfo.User;
            set
            {
                secureServerInfo.User = value;
                RaisePropertyChanged();
            }
        }


        public string Password
        {
            get => secureServerInfo.Password.ToUnsecureString() ?? string.Empty;
            set
            {
                secureServerInfo.Password = value.ToSecureString();
                RaisePropertyChanged();
            }
        }


        public string PrivateKey
        {
            get => secureServerInfo.PrivateKey?.ToUnsecureString() ?? string.Empty;
            set
            {
                secureServerInfo.PrivateKey = value.ToSecureString();
                RaisePropertyChanged();
            }
        }


        public bool CanCloseDialog()
        {
            return true;
        }


        public void OnDialogClosed()
        {
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(SecureServerInfo)))
            {
                this.secureServerInfo = parameters.GetValue<SecureServerInfo>(nameof(SecureServerInfo));
            }
        }


        /// <summary>
        ///     OKボタンコマンド
        /// </summary>
        public DelegateCommand OkCommand => new(() =>
        {
            var parameters = new DialogParameters();

            parameters.Add(nameof(SecureServerInfo), secureServerInfo);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
        });


        /// <summary>
        ///     キャンセルボタンコマンド
        /// </summary>
        public DelegateCommand CancelCommand => new(() =>
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        });
    }
}
