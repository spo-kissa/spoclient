using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using spoclient.Service;

namespace spoclient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }


    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        ClipboardService.Owner = TopLevel.GetTopLevel(this);


        if (e.Root is AppWindow aw)
        {
            (aw.SplashScreen as MainAppSplashScreen)!.InitApp += () =>
            {
                InitializeNavigationPages();
            };
        }
        else
        {
            InitializeNavigationPages();
        }
    }


    public void InitializeNavigationPages()
    {

    }
}