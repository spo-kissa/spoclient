using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient
{
    internal class MainAppSplashScreen : IApplicationSplashScreen
    {
        public string? AppName { get; } = null;


        public IImage? AppIcon { get; } = null;


        public object SplashScreenContent { get; }


        public int MinimumShowTime { get; set; }


        public Action? InitApp { get; set; }


        public MainAppSplashScreen()
        {
            SplashScreenContent = new MainAppSplashContent();
        }


        public async Task RunTasks(CancellationToken cancellationToken)
        {
            await ((MainAppSplashContent)SplashScreenContent).InitApp();
        }
    }
}
