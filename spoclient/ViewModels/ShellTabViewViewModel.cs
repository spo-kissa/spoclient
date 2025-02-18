using System;
using System.Collections.Generic;
using Prism.Mvvm;
using spoclient.Models;

namespace spoclient.ViewModels
{
    public class ShellTabViewViewModel : MainTabViewModel
    {
        public override string Header => "V‚µ‚¢Ú‘±";


        public ServerInfo? ServerInfo { get; set; }
    }
}