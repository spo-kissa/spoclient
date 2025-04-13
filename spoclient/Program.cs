using Avalonia;
using LocalizationManager;
using LocalizationManager.Avalonia;
using SpoClient.Localization;
using System;
using System.Resources;

namespace spoclient
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() => AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
                .UseLocalizationManager(() =>
                {
                    var manager = new ResourceManager(
                        "SpoClient.Localization.Resources.Strings",
                        typeof(Localizer).Assembly
                    );
                    return LocalizationProviderExtensions.MakeResourceProvider("UI", manager);
                })
                .WithInterFont()
                .LogToTrace()
                .UseSkia()
            ;
    }
}
