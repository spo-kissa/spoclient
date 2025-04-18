using Avalonia.Markup.Xaml;
using LocalizationManager;
using Prism.Ioc;
using System;

namespace spoclient.Views.Extensions
{
    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; }


        public string DefaultValue { get; set; }


        public TranslateExtension(string key)
        {
            Key = key;
            DefaultValue = key;
        }


        public TranslateExtension(string key, string defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                var localizationManager = ContainerLocator.Container.Resolve<ILocalizationManager>();
                if (localizationManager == null)
                {
                    return DefaultValue;
                }
                return localizationManager.GetValue(Key, "UI");
            }
            catch
            {
                return DefaultValue;
            }
        }
    }
}
