using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SpoDashboad.ViewModels;
using SpoDashboad.Views;
using Prism.DryIoc;
using Prism.Ioc;

namespace SpoDashboad;

public partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
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

    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}