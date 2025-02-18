using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using spoclient.Views;

namespace spoclient;

public partial class ShellTabView : UserControl, IMainTabViewItem
{
    public ShellTabView()
    {
        InitializeComponent();
    }

    public int TabItemIndex { get; set; }

    public bool IsStartupTab { get; set; }
}