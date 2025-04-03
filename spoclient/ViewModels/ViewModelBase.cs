using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm;

namespace spoclient.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        protected string title = string.Empty;

        public string Title { get => title; set => SetProperty(ref title, value); }

    }
}
