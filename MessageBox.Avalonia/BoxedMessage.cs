
using MessageBoxSlim.Avalonia.DTO;
using MessageBoxSlim.Avalonia.Interfaces;
using MessageBoxSlim.Avalonia.ViewModels;
using MessageBoxSlim.Avalonia.Views;


namespace MessageBoxSlim.Avalonia
{
    public static class BoxedMessage
    {
        public static IMessageBox<UserResult> Create(MessageBoxParams @params)
        {
            var vm = new MessageBoxViewModel(@params);
            MessageBoxWindow window = new MessageBoxWindow(@params.Style);
            window.DataContext = vm;
            vm.Window = window;
            return window;
        }
    }
}
