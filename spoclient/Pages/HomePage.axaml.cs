using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using spoclient.ViewModels;

namespace spoclient.Pages;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();

        var vm = new HomePageViewModel();

        DataContext = vm;
    }
}