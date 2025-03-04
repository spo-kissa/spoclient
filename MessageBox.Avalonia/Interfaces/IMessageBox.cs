using Avalonia.Controls;

using System.Threading.Tasks;

namespace MessageBoxSlim.Avalonia.Interfaces
{
    public interface IMessageBox<T>
    {
        Task<T> ShowDialogAsync(Window ownerWindow);
        Task<T> ShowAsync();
    }
}