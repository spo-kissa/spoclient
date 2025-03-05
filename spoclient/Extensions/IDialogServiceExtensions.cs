using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Prism.Services.Dialogs;
using System;
using System.Linq;

namespace spoclient.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowDialog<T>(this IDialogService dialogService, IDialogParameters? parameters, Action<IDialogResult> callback)
        {
            var owner = GetActiveWindow();
            if (owner is not null)
            {
                var type = typeof(T);
                dialogService.ShowDialog(owner, type.Name, parameters, callback);
            }
            else
            {
                dialogService.ShowDialog(nameof(T), parameters, callback);
            }
        }


        private static Window? GetActiveWindow()
        {
            return Application.Current?.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => desktop.Windows.FirstOrDefault(w => w.IsActive),
                _ => null,
            };
        }
    }
}
