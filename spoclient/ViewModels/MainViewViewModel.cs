using Avalonia.Controls;
using DryIoc;
using Prism.Commands;
using Prism.Services.Dialogs;
using spoclient.Models;
using spoclient.ViewModels.MainView;
using spoclient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace spoclient.ViewModels
{
    public class MainViewViewModel : ViewModelBase
    {
        public NavigationFactory NavigationFactory { get; }


        public ObservableCollection<MainTabViewModel> Connections { get; } = [];


        private Dictionary<Type, Type> TabViewModelMatcher { get; } = [];


        public int? SelectedIndex
        {
            get => selectedIndex;
            set => SetProperty(ref selectedIndex, value);
        }


        private int? selectedIndex;


        private readonly IDialogService dialogService;


        private readonly IContainer container;
        

        public MainViewViewModel(IDialogService dialogService, IContainer container)
        {
            this.dialogService = dialogService;
            this.container = container;

            NavigationFactory = new NavigationFactory(this);


            TabViewModelMatcher.Add(typeof(ShellTabView), typeof(ShellTabViewViewModel));
        }


        public DelegateCommand ConnectCommand => new(() =>
        {
            dialogService.ShowDialog(nameof(ServersDialog), dialogResult =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var serverInfo = dialogResult.Parameters.GetValue<SecureServerInfo>("ServerInfo");

                    var tuple = CreateTabView<ShellTabViewViewModel, ShellTabView>();
                    
                    if (tuple is not null && serverInfo is not null)
                    {
                        Connections.Add(tuple!.Value.ViewModel);
                        tuple!.Value.ViewModel.Connect(serverInfo);

                        tuple!.Value.ViewModel.RequestClose += TabViewRequestClose;

                        SelectedIndex = Connections.Count - 1;
                    }
                }
            });
        });


        private void TabViewRequestClose(MainTabViewModel mainTabViewModel)
        {
            mainTabViewModel.RequestClose -= TabViewRequestClose;
            var index = Connections.IndexOf(mainTabViewModel);

            Connections.RemoveAt(index);
        }



        private (TView View, TViewModel ViewModel)? CreateTabView<TViewModel, TView>()
            where TView : UserControl, IMainTabViewItem, new()
            where TViewModel : MainTabViewModel
        {
            var result = TabViewModelMatcher.ContainsKey(typeof(TView));
            if (result)
            {
                var viewModelType = TabViewModelMatcher[typeof(TView)];
                var viewModel = container.Resolve<TViewModel>(); // (TViewModel)Activator.CreateInstance(viewModelType)!;
                var view = Activator.CreateInstance<TView>();
                viewModel.Content = view;
                view.DataContext = viewModel;

                return (view, viewModel);
            }

            return null;
        }
    }
}
