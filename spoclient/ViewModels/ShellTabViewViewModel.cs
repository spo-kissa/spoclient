using System;
using System.Collections.Generic;
using System.ComponentModel;
using Prism.Mvvm;
using spoclient.Models;
using spoclient.Service;

namespace spoclient.ViewModels
{
    public class ShellTabViewViewModel : MainTabViewModel
    {
        public override string Header { get => header; }


        public ServerInfo? ServerInfo { get; private set; }


        private string header = string.Empty;


        private SshService? SshService { get; set; }


        public string TerminalOutput
        {
            get => terminalOutput;
            set
            {
                terminalOutput = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(TerminalOutput)));
            }
        }


        private string terminalOutput = "";





        public ShellTabViewViewModel()
        {
        }


        public void SetHeader(string header)
        {
            this.header = header;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Header)));
        }


        public async void Connect(ServerInfo serverInfo)
        {
            ServerInfo = serverInfo;

            SshService = new SshService(serverInfo);
            SshService.Output += OnSshOutput;
            SshService.StateChanged += OnSshStateChanged;
            await SshService.ConnectAsync();
        }

        private void OnSshStateChanged(object? sender, SshStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case SshState.Connecting:
                    SetHeader("接続しています...");
                    break;

                case SshState.Connected:
                    SetHeader(ServerInfo!.Server);
                    break;

            }
        }

        private void OnSshOutput(object? sender, SshOutputEventArgs e)
        {
            TerminalOutput += e.Output;
        }
    }
}