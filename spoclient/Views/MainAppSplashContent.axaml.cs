using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using spoclient.Plugins.Recipe;
using System.Threading.Tasks;

namespace spoclient;

public partial class MainAppSplashContent : UserControl
{
    public MainAppSplashContent()
    {
        InitializeComponent();
    }


    public async Task InitApp()
    {
        var progressValue = 0;

        LoadingText.Text = "Loading Plugins...";

        var plugins = RecipeV1Loader.FindRecipePlugins();
        var recipes = RecipeV1Loader.GetRecipes(plugins);
        RecipeV1Loader.RegisterRecipes(recipes);

        while (progressValue < 100)
        {
            progressValue++;
            Dispatcher.UIThread.Post(() => ProgressBar.Value = progressValue);
            await Task.Delay(20);
        }
    }
}