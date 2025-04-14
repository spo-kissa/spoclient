using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using LocalizationManager;
using System;
using System.Globalization;
namespace SpoClient.Localization
{


    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; }
        private ILocalizationManager localizationManager;
        public TranslateExtension(ILocalizationManager localizationManager) 
        {
            this.localizationManager = localizationManager;
        }

        public TranslateExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return StaticLocalizer.Get(Key);
        }
    }

}
