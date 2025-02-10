using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using spoclient.Models;

namespace spoclient.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public ICommand ConnectRequestedCommand { get; }


        public HomePageViewModel()
        {
            ConnectRequestedCommand = new RelayCommand<ServerInfo>((e) =>
            {
                System.Diagnostics.Debug.WriteLine("OK");
            });
        }
    }
}
