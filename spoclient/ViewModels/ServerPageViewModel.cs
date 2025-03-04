using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using spoclient.Models;

namespace spoclient.ViewModels
{
	public class ServerPageViewModel : ViewModelBase
	{
		private readonly ObservableCollection<SecureServerInfo> servers;

		public event EventHandler<SecureServerInfo>? ConnectRequest;


		public ICommand ConnectCommand { get; }

		public IRelayCommand<SelectionChangedEventArgs> SelectionChangedCommand { get; }


		public IReadOnlyList<SecureServerInfo> Servers
		{
			get => servers.AsReadOnly();
		}


		public SecureServerInfo? SelectedServer { get; private set; }



		public ServerPageViewModel()
		{
			var raw = "apJqUK57";
			var password = new SecureString();
			foreach (var ch in raw)
			{
				password.AppendChar(ch);
			}

			servers = [
				new SecureServerInfo("Hyper-V", "172.27.221.2", "daisuke", password, "22"),
			];

			ConnectCommand = new RelayCommand(() =>
			{
				if (SelectedServer is not null)
				{
					ConnectRequest?.Invoke(this, SelectedServer);
				}
			});

			SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>((e) =>
			{
                if (e is not null)
                {
                    var count = (e.AddedItems.Count - e.RemovedItems.Count);
                    if (count == 1)
                    {
                        SelectedServer = e.AddedItems[0] as SecureServerInfo;
						return;
                    }
                }
				SelectedServer = null;
			});
		}
	}
}