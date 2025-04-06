using CommunityToolkit.Mvvm.Input;
using SpoClient.Setting.Models;
using System.Windows.Input;

namespace spoclient.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ICommand ConnectRequestedCommand { get; }


        public HomePageViewModel()
        {
            ConnectRequestedCommand = new RelayCommand<SecureServer>((e) =>
            {
                System.Diagnostics.Debug.WriteLine("OK");
            });
        }
    }
}
