using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using spoclient.Pages;
using spoclient.Regions;
using spoclient.ViewModels;
using spoclient.Views;

namespace spoclient
{
    public partial class App : PrismApplication
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }


        protected override AvaloniaObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();

            containerRegistry.RegisterForNavigation<MainView, MainViewViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();

            containerRegistry.RegisterDialogWindow<DialogWindow>();

            containerRegistry.RegisterDialog<ServersDialog, ServersDialogViewModel>();
        }


        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }


        protected override void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("RootRegion", typeof(MainView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomePage));
            regionManager.RegisterViewWithRegion("TabView", typeof(TabView));
        }


        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping<ContentControl, ContentControlRegionAdapter>();
            regionAdapterMappings.RegisterMapping<TabView, MainTabControlRegionAdapter>();
        }



        //public override void OnFrameworkInitializationCompleted()
        //{
        //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //    {
        //        // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
        //        // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
        //        DisableAvaloniaDataAnnotationValidation();
        //        desktop.MainWindow = new MainWindow
        //        {
        //            DataContext = new MainWindowViewModel(),
        //        };
        //    }

        //    base.OnFrameworkInitializationCompleted();
        //}

        //private void DisableAvaloniaDataAnnotationValidation()
        //{
        //    // Get an array of plugins to remove
        //    var dataValidationPluginsToRemove =
        //        BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        //    // remove each entry found
        //    foreach (var plugin in dataValidationPluginsToRemove)
        //    {
        //        BindingPlugins.DataValidators.Remove(plugin);
        //    }
        //}
    }
}