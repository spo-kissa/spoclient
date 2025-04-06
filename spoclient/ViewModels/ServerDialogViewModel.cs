using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Prism.Commands;
using Prism.Services.Dialogs;
using spoclient.Extensions;
using SpoClient.Setting.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace spoclient.ViewModels
{
    /// <summary>
    ///     サーバー情報ダイアログのViewModel
    /// </summary>
    public class ServerDialogViewModel : ViewModelBase, IDialogAware
    {
        /// <summary>
        ///     ダイアログが閉じられるときに発生するイベント
        /// </summary>
        public event Action<IDialogResult>? RequestClose;


        /// <summary>
        ///     サーバー情報
        /// </summary>
        private SecureServer secureServer;


        /// <summary>
        ///     エントリ名
        /// </summary>
        public string Name
        {
            get => secureServer.Name;
            set
            {
                secureServer.Name = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     サーバー名
        /// </summary>
        public string Host
        {
            get => secureServer.Host;
            set
            {
                secureServer.Host = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     ポート番号
        /// </summary>
        public string Port
        {
            get => secureServer.Port;
            set
            {
                secureServer.Port = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     ユーザー名
        /// </summary>
        public string User
        {
            get => secureServer.User;
            set
            {
                secureServer.User = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     パスワード
        /// </summary>
        public string Password
        {
            get => secureServer.Password?.ToUnsecureString() ?? string.Empty;
            set
            {
                secureServer.Password = value.ToSecureString();
                RaisePropertyChanged();
            }
        }


        /// <summary>
        ///     秘密鍵
        /// </summary>
        public string? PrivateKey
        {
            get => secureServer.PrivateKey?.ToUnsecureString();
            set
            {
                secureServer.PrivateKey = value?.ToSecureString();
                RaisePropertyChanged();
            }
        }



        public ServerDialogViewModel()
        {
            secureServer = SecureServer.Empty;
            Title = "Add Server Entry";
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
            if (parameters.ContainsKey(nameof(SecureServer)))
            {
                this.secureServer = parameters.GetValue<SecureServer>(nameof(SecureServer));
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(Host));
                RaisePropertyChanged(nameof(Port));
                RaisePropertyChanged(nameof(User));
                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(PrivateKey));

                this.Title = "Edit Server Entry";
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
        public DelegateCommand<UserControl> ImportPrivateKeyClipboadCommand => new(async (e) =>
        {
            var topLevel = TopLevel.GetTopLevel(e);

            if (topLevel is null)
            {
                return;
            }

            if (topLevel.Clipboard is null)
            {
                return;
            }

            var formats = await topLevel.Clipboard.GetFormatsAsync();

            if (formats is null)
            {
                return;
            }


            foreach (var format in formats)
            {
                try
                {
                    var data = await topLevel.Clipboard.GetDataAsync(format);
                    System.Diagnostics.Debug.WriteLine("==================================================================");
                    System.Diagnostics.Debug.WriteLine(format);
                    System.Diagnostics.Debug.WriteLine(data);


                    if (format == "Files" && data is IEnumerable<IStorageItem> enumerator)
                    {
                        foreach (var item in enumerator)
                        {
                            System.Diagnostics.Debug.WriteLine(item.Name);
                        }
                    }
                    if (format == "FileName" && data != null)
                    {
                        var fileName = Encoding.Default.GetString((byte[])data).Trim();
                        System.Diagnostics.Debug.WriteLine(fileName);
                        using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                        using var streamReader = new StreamReader(fileStream);
                        PrivateKey = await streamReader.ReadToEndAsync();
                    }
                    if (format == "Text" && data != null)
                    {
                        PrivateKey = data?.ToString();
                    }
                }
                catch { }
            }

        }, (e) =>
        {
            var clipboard = TopLevel.GetTopLevel(e)?.Clipboard;
            if (clipboard is null)
            {
                return false;
            }
            var formats = clipboard.GetFormatsAsync().Result;
            return formats.Length > 0;
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
        public DelegateCommand<UserControl> ExportPrivateKeyClipboadCommand => new(async (e) =>
        {
            var topLevel = TopLevel.GetTopLevel(e);

            if (topLevel is null)
            {
                return;
            }

            if (topLevel.Clipboard is null)
            {
                return;
            }

            await topLevel.Clipboard.SetTextAsync(PrivateKey);

        }, (e) =>
        {
            return !string.IsNullOrWhiteSpace(PrivateKey);
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
            result.Parameters.Add(nameof(SecureServer), secureServer);

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
