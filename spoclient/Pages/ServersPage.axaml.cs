using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Xaml.Interactions.Core;
using CommunityToolkit.Mvvm.Input;
using spoclient.Models;
using spoclient.ViewModels;

namespace spoclient.Pages;

public partial class ServersPage : UserControl
{
    public ICommand? ConnectRequestCommand
    {
        get => GetValue(ConnectRequestCommandProperty);
        set => SetValue(ConnectRequestCommandProperty, value);
    }


    public static readonly StyledProperty<ICommand?> ConnectRequestCommandProperty =
        AvaloniaProperty.Register<ServersPage, ICommand?>(nameof(ConnectRequestCommand));


    public ServersPage()
    {
        InitializeComponent();

        var vm = new ServerPageViewModel();

        vm.ConnectRequest += (s, e) =>
        {
            ConnectRequestCommand?.Execute(null);
        };

        DataContext = vm;
    }
}