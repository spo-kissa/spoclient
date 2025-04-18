using Avalonia.Markup.Xaml;
using LocalizationManager;
using Prism.Ioc;
using System;

namespace spoclient.Views.Extensions
{
    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; }


        public TranslateExtension(string key)
        {
            Key = key;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var localizationManager = ContainerLocator.Container.Resolve<ILocalizationManager>();
            return localizationManager.GetValue(Key, "UI");
        }
    }
}
