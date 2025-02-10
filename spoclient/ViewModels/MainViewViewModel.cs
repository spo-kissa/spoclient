using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using spoclient.Models;
using spoclient.ViewModels.MainView;

namespace spoclient.ViewModels
{
    public class MainViewViewModel : ViewModelBase
    {
        public NavigationFactory NavigationFactory { get; }


        public ObservableCollection<ConnectionInfo> Connections { get; } = [];
 
        

        public MainViewViewModel()
        {
            NavigationFactory = new NavigationFactory(this);

            ConnectCommand = new RelayCommand(() =>
            {
                var connection = new ConnectionInfo();
                Connections.Add(connection);
            });
        }


        public ICommand ConnectCommand { get; }

    }
}
