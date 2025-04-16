using Avalonia.Markup.Xaml;
using LocalizationManager;
using System;
using Prism.Ioc;
namespace SpoClient.Localization
{


    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; }

        public TranslateExtension()
        {
        }

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
