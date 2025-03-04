using MessageBoxSlim.Avalonia.Interfaces;
using MessageBoxSlim.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace spoclient.Extensions
{
    public static class IMessageBoxExtensions
    {
        public static async Task<UserResult> ShowDialogAsync(this IMessageBox<UserResult> messageBox)
        {
            var owner = GetActiveWindow();
            if (owner is not null)
            {
                return await messageBox.ShowDialogAsync(owner);
            }
            else
            {
                return await messageBox.ShowAsync();
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
