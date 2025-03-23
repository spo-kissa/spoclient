using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using spoclient.Extensions;
using spoclient.Models;
using System;
using System.IO;
using System.Text;

namespace spoclient.ViewModels
{
    /// <summary>
    ///     サーバー情報ダイアログのViewModel
    /// </summary>
    public class ServerDialogViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        ///     ダイアログが閉じられるときに発生するイベント
        /// </summary>
        public event Action<IDialogResult>? RequestClose;


        /// <summary>
        ///     ウィンドウタイトル
        /// </summary>
        private string title = "Server New Entry";


        /// <summary>
        ///     サーバー情報
        /// </summary>
        private SecureServerInfo secureServerInfo = new();



        /// <summary>
        ///     ウィンドウタイトル
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     エントリ名
        /// </summary>
        public string Entry
        {
            get => secureServerInfo.Entry;
            set
            {
                secureServerInfo.Entry = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     サーバー名
        /// </summary>
        public string Server
        {
            get => secureServerInfo.Server;
            set
            {
                secureServerInfo.Server = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     ポート番号
        /// </summary>
        public string Port
        {
            get => secureServerInfo.Port;
            set
            {
                secureServerInfo.Port = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     ユーザー名
        /// </summary>
        public string User
        {
            get => secureServerInfo.User;
            set
            {
                secureServerInfo.User = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     パスワード
        /// </summary>
        public string Password
        {
            get => secureServerInfo.Password.ToUnsecureString() ?? string.Empty;
            set
            {
                secureServerInfo.Password = value.ToSecureString();
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     秘密鍵
        /// </summary>
        public string? PrivateKey
        {
            get => secureServerInfo.PrivateKey?.ToUnsecureString();
            set
            {
                secureServerInfo.PrivateKey = value?.ToSecureString();
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     ダイアログが閉じることができるかどうかを取得します
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }


        /// <summary>
        ///     ダイアログが閉じられたときに呼び出されます
        /// </summary>
        public void OnDialogClosed()
        {
        }


        /// <summary>
        ///     ダイアログが開かれたときに呼び出されます
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(nameof(SecureServerInfo)))
            {
                this.secureServerInfo = parameters.GetValue<SecureServerInfo>(nameof(SecureServerInfo));
                RaisePropertyChanged(nameof(Entry));
                RaisePropertyChanged(nameof(Server));
                RaisePropertyChanged(nameof(Port));
                RaisePropertyChanged(nameof(User));
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(PrivateKey));

                this.Title = "Server Edit Entry";
            }
        }


        /// <summary>
        ///     ファイルから秘密鍵をインポートメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ImportPrivateKeyFileCommand => new(async (e) =>
        {
            var topLevel = TopLevel.GetTopLevel(e);

            if (topLevel is null)
            {
                return;
            }

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open SSH Private Key File",
                AllowMultiple = false,
            });

            if (files.Count >= 1)
            {
                await using var stream = await files[0].OpenReadAsync();
                using var streamReader = new StreamReader(stream);
                var fileContent = await streamReader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(fileContent))
                {
                    return;
                }

                PrivateKey = fileContent;
            }
        }, (e) =>
        {
            return true;
        });


        /// <summary>
        ///     クリップボードから秘密鍵をインポートメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ImportPrivateKeyClipboadCommand => new((e) =>
        {
        }, (e) =>
        {
            return false;
        });



        /// <summary>
        ///     エクスポートメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ExportCommand => new((e) =>
        {
        }, (e) =>
        {
            return !string.IsNullOrWhiteSpace(PrivateKey);
        });



        /// <summary>
        ///     ファイルへ秘密鍵をエクスポートメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ExportPrivateKeyFileCommand => new(async (e) =>
        {
            var topLevel = TopLevel.GetTopLevel(e);

            if (topLevel is null)
            {
                return;
            }

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Export SSH Private Key File",
                DefaultExtension = ".key",
                ShowOverwritePrompt = true,
                SuggestedFileName = "private.key",
            });

            if (file is not null && PrivateKey is not null)
            {
                await using var stream = await file.OpenWriteAsync();
                using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
                await streamWriter.WriteAsync(PrivateKey);
            }
        }, (e) =>
        {
            return !string.IsNullOrWhiteSpace(PrivateKey);
        });


        /// <summary>
        ///     クリップボードへ秘密鍵をエクスポートメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ExportPrivateKeyClipboadCommand => new((e) =>
        {
        }, (e) =>
        {
            return false;
        });



        /// <summary>
        ///     クリアメニューコマンド
        /// </summary>
        public DelegateCommand<UserControl> ClearPrivateKeyCommand => new((e) =>
        {
            PrivateKey = null;
        }, (e) =>
        {
            return !string.IsNullOrWhiteSpace(PrivateKey);
        });


        /// <summary>
        ///     OKボタンコマンド
        /// </summary>
        public DelegateCommand OkCommand => new(() =>
        {
            var result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add(nameof(SecureServerInfo), secureServerInfo);

            RequestClose?.Invoke(result);
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
