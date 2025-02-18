using System;
using System.Collections.Generic;
using Prism.Mvvm;
using spoclient.Models;

namespace spoclient.ViewModels
{
    public class ShellTabViewViewModel : MainTabViewModel
    {
        public override string Header => "新しい接続";


        public ServerInfo? ServerInfo { get; set; }
    }
}