using Prism.Commands;
using Prism.Mvvm;
using System;

namespace spoclient.ViewModels
{
    public abstract class MainTabViewModel : BindableBase
    {
        public delegate void RequestCloseEventHandler(MainTabViewModel source);

        public abstract event RequestCloseEventHandler RequestClose;


        public abstract string Header { get; }


        public object? Content { get; set; } = null;


        public abstract DelegateCommand CloseCommand { get; }
    }
}
