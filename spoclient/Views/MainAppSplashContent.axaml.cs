using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using spoclient.Plugins.Recipe;
using SpoClient.Setting;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace spoclient;

public partial class MainAppSplashContent : UserControl
{
    public MainAppSplashContent()
    {
        InitializeComponent();

        this.AttachedToVisualTree += async (_, __) => await RunLogoAnimation();
    }


    public async Task InitApp()
    {
        var progressValue = 0;

        LoadingText.Text = "Loading Settings...";

        var settings = Settings.Instance;
        settings.PasswordRequired += ((s, e) =>
        {
            var password = new SecureString();
            foreach (var c in "spoclient")
            {
                password.AppendChar(c);
            }
            settings.Password = password;
        });
        await settings.OpenAsync("spoclient.setting");


        var migrator = new Migrator(settings.Connection!);
        await migrator.ApplyMigrationsFromResourcesAsync("SpoClient.Setting.Servers.Migrations");


        LoadingText.Text = "Loading Plugins...";

        var plugins = RecipeV1Loader.FindRecipePlugins();
        var recipes = RecipeV1Loader.GetRecipes(plugins);
        RecipeV1Loader.RegisterRecipes(recipes);

        while (progressValue < 100)
        {
            progressValue++;
            Dispatcher.UIThread.Post(() => ProgressBar.Value = progressValue);
            await Task.Delay(15);
        }
    }



    private async Task RunLogoAnimation()
    {
        var logo = this.FindControl<Image>("logo_image");
        if (logo is null)
        {
            return;
        }

        var tg = logo.RenderTransform as TransformGroup;
        if (tg is null)
        {
            return;
        }

        var scale = tg.Children[0] as ScaleTransform;
        var rotate = tg.Children[1] as RotateTransform;

        // Opacity animation
        var opacityAnimation = CreateAnimation(1.5, new CubicEaseOut(), FillMode.Forward,
            (0d, Visual.OpacityProperty, 0),
            (1d, Visual.OpacityProperty, 1)
        );

        // Scale animation
        var scaleAnimation = CreateAnimation(1.5, new BackEaseOut(), FillMode.Forward,
            (0d, ScaleTransform.ScaleXProperty, 0.5),
            (1d, ScaleTransform.ScaleXProperty, 1.0)
        );

        var scaleYAnimation = CreateAnimation(1.5, new BackEaseOut(), FillMode.Forward,
            (0d, ScaleTransform.ScaleYProperty, 0.5),
            (1d, ScaleTransform.ScaleYProperty, 1.0)
        );

        // Rotate animation
        var rotateAnimation = CreateAnimation(1.5, new CubicEaseOut(), FillMode.Forward,
            (0d, RotateTransform.AngleProperty, -20),
            (1d, RotateTransform.AngleProperty, 0)
        );

        await Task.WhenAll(
            opacityAnimation.RunAsync(logo),
            scaleAnimation.RunAsync(logo),
            scaleYAnimation.RunAsync(logo),
            rotateAnimation.RunAsync(logo)
        );
    }


    private static KeyFrame CreateKeyFrame(double cue, Setter setter)
    {
        var keyFrame = new KeyFrame();
        keyFrame.Cue = new Cue(cue);
        keyFrame.Setters.Add(setter);

        return keyFrame;
    }


    private static Animation CreateAnimation(double seconds, Easing easing, FillMode fillMode, params (double cue, AvaloniaProperty property, object? value)[] keyFrames)
    {
        return CreateAnimation(TimeSpan.FromSeconds(seconds), easing, fillMode, [.. keyFrames.Select(k => CreateKeyFrame(k.cue, new Setter(k.property, k.value)))]);
    }


    private static Animation CreateAnimation(double seconds, Easing easing, FillMode fillMode, params (double cue, Setter setter)[] keyFrames)
    {
        var animation = CreateAnimation(TimeSpan.FromSeconds(seconds), easing, fillMode);

        foreach (var (cue, setter) in keyFrames)
        {
            animation.Children.Add(CreateKeyFrame(cue, setter));
        }

        return animation;
    }


    private static Animation CreateAnimation(TimeSpan duration, Easing easing, FillMode fillMode, params KeyFrame[] keyFrames)
    {
        var animation = new Animation
        {
            Duration = duration,
            Easing = easing,
            FillMode = fillMode,
        };

        animation.Children.AddRange(keyFrames);

        return animation;
    }
}