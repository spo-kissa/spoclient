using System;
using System.Globalization;
using System.Resources;

namespace SpoClient.Localization
{
    public static class StaticLocalizer
    {
        private static ResourceManager _resourceManager { get; } =
            new ResourceManager("SpoClient.Localization.Resources.Strings", typeof(Localizer).Assembly);

        private static CultureInfo _currentCulture = new CultureInfo(StaticLocalizationSettings.GetCulture());
        private static CultureInfo _defaultCulture = new CultureInfo("en-US");

        public static string Get(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                    return string.Empty;
                var localizedString = _resourceManager.GetString(key, _currentCulture);
                return localizedString ?? key;
            }
            catch (Exception ex)
            {
                return _resourceManager.GetString(key, _defaultCulture) ?? key;
            }
        }

    }
}
