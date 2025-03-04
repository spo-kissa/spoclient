using Avalonia;
using Avalonia.Controls;
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
    }
}