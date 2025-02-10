using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Renci.SshNet;
using spoclient.Service;
using spoclient.ViewModels;
using FluentAvalonia.UI.Controls;

namespace spoclient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        var vm = new MainViewViewModel();
        DataContext = vm;
    }


    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        ClipboardService.Owner = TopLevel.GetTopLevel(this);
    }



    public ConnectionInfo ConnectionInfo { private set; get; }

    public string HostName { private set; get; }

    public Int32 Port { private set; get; }

    public string UserName { private set; get; }

    public string Password { private set; get; }



    private void OnClick(object sender, RoutedEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("OK");

        HostName = "relay4.mobc.work";
        Port = 18134;
        UserName = "cardano";
        Password = "";

        var KeyFile = @"..\..\..\id_ed25519";
        var PassPhrase = "apJqUK57";

        var PasswordAuth = new PasswordAuthenticationMethod(UserName, Password);

        var PrivateKey = new PrivateKeyAuthenticationMethod(UserName,
        [
            new PrivateKeyFile(KeyFile, PassPhrase)
        ]);

        ConnectionInfo = new ConnectionInfo(HostName, Port, UserName,
            [
                PasswordAuth,
                PrivateKey
            ]
        );


        using (var ssh = new SshClient(ConnectionInfo))
        {
            ssh.Connect();

            var command = ssh.CreateCommand("ls -la");
            var result = command.Execute();

            System.Diagnostics.Debug.WriteLine($"{result}");

            ssh.Disconnect();
        }
    }
}