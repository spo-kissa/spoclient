using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;

using MessageBoxSlim.Avalonia.DTO;
using MessageBoxSlim.Avalonia.Enums;
using MessageBoxSlim.Avalonia.Views;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoxSlim.Avalonia.ViewModels
{
    public class MessageBoxViewModel : ViewModelBase
    {
        private readonly MessageBoxParams _dtoParams;

        public MessageBoxViewModel(MessageBoxParams @params)
        {
            if (@params.Icon == null)
            {
                @params.Icon = BitmapFactory.Load("avares://MessageBoxSlim.Avalonia/Assets/noicon.ico");
            }

            _dtoParams = @params;
        }

        internal MessageBoxWindow Window { get; set; }

        public bool OkButton => _dtoParams.Buttons.HasFlag(ButtonEnum.Ok);

        public bool YesButton => _dtoParams.Buttons.HasFlag(ButtonEnum.Yes);

        public bool NoButton => _dtoParams.Buttons.HasFlag(ButtonEnum.No);

        public bool AbortButton =>
            _dtoParams.Buttons.HasFlag(ButtonEnum.Abort);

        public bool CancelButton =>
            _dtoParams.Buttons.HasFlag(ButtonEnum.Cancel);

        public bool CanResize => _dtoParams.CanResize;

        public bool HasIcon => _dtoParams.Icon != null;

        public string Title => _dtoParams.ContentTitle;

        public string Message => _dtoParams.ContentMessage;

        public Bitmap Icon => _dtoParams.Icon;

        public int? MaxWidth => _dtoParams.MaxWidth;

        public WindowStartupLocation Location => _dtoParams.Location;

        public async Task CopyToClipboard()
        {
            var window = GetActiveWindow();
            if (window == null)
            {
                return;
            }

            await window.Clipboard.SetTextAsync(Message);
        }

        public void ButtonClick(string parameter)
        {
            Window.UserResult =
                (UserResult)
                Enum.Parse(typeof(UserResult), parameter.Trim(), false);
            Window.Close();
        }


        private static Window GetActiveWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.Windows.FirstOrDefault(w => w.IsActive);
            }
            return null;
        }
    }
}
