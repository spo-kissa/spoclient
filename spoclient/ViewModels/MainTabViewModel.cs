using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.ViewModels
{
    public abstract class MainTabViewModel : BindableBase
    {
        public abstract string Header { get; }


        public object? Content { get; set; } = null;

    }
}
