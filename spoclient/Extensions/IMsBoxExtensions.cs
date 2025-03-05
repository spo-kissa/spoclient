using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace spoclient.Extensions
{
    public static class IMsBoxExtensions
    {
        public static async Task<ButtonResult> ShowDialogAsync(this IMsBox<ButtonResult> box)
        {
            var owner = GetActiveWindow();
            if (owner is not null)
            {
                return await box.ShowWindowDialogAsync(owner);
            }
            else
            {
                return await box.ShowAsync();
            }
        }


        public static async Task<ButtonResult> ShowPopupAsync(this IMsBox<ButtonResult> box)
        {
            var owner = GetActiveWindow();
            if (owner is not null)
            {
                return await box.ShowAsPopupAsync(owner);
            }
            else
            {
                return await box.ShowAsync();
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
