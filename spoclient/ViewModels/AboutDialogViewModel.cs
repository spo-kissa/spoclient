using LocalizationManager;
using Prism.Commands;
using Prism.Services.Dialogs;
using spoclient.Plugins.Recipe;
using SpoClient.Plugin.Recipe.V1;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace spoclient.ViewModels
{
    public class AboutDialogViewModel : ViewModelBase, IDialogAware
    {
        public event Action<IDialogResult>? RequestClose;


        private string version = string.Empty;

        ILocalizationManager localization;
        public string Version
        {
            get => version;
            set
            {
                version = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<IRecipePlugin> Plugins { get; } = [];


        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            var result = new DialogResult(ButtonResult.OK);
            RequestClose?.Invoke(result);
        });


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public AboutDialogViewModel(ILocalizationManager localization)
        {
            Version = GetAssemblyVersion(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())?.ToString() ?? string.Empty;
            this.localization = localization;
            var plugins = RecipeV1Loader.FindRecipePlugins();
            foreach (var plugin in plugins)
            {
                if (plugin is not null)
                {
                    Plugins.Add(plugin);
                }
            }
        }


        private Version? GetAssemblyVersion(Assembly assembly)
        {
            var location = assembly.Location;
            return assembly.GetName().Version;
        }


        public bool CanCloseDialog()
        {
            return true;
        }


        public void OnDialogClosed()
        {
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = localization["AppInfo","UI"];
        }
    }
}
