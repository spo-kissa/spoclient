using System;
using System.IO;
using Newtonsoft.Json;

public static class StaticLocalizationSettings
{
    private static readonly string _settingsFilePath = "appsettings.json";

    public  static string GetCulture()
    {
        if (File.Exists(_settingsFilePath))
        {
            var json = File.ReadAllText(_settingsFilePath);
            var settings = JsonConvert.DeserializeObject<Settings>(json);
            return settings.Localization.Culture;
        }
        return "en-US";  // Default culture
    }

    public static  void SetCulture(string culture)
    {

        try
        {
            var settings = new Settings { Localization = new LocalizationSettings { Culture = culture } };
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            if (File.Exists(_settingsFilePath))
            {
                File.Delete(_settingsFilePath); 
            }

            File.WriteAllText(_settingsFilePath, json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error writtig: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Error of acces: {ex.Message}");
        }
    }
    private class Settings
    {
        public LocalizationSettings Localization { get; set; }
    }
    private class LocalizationSettings
    {
        public string Culture { get; set; }
    }
}


