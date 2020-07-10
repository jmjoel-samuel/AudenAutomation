using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AudenAutomationTests.Helper
{
    public class ConfigHelper
    {
        private static ConfigHelper appSettings;

        public ConfigHelper(IConfiguration config, string key)
        {
            if (config != null)
            {
                this.AppSettingValue = config[key];
            }
        }

        public string AppSettingValue { get; set; }

        public static string AppSetting(string key)
        {
            appSettings = GetCurrentSettings(key);
            return appSettings.AppSettingValue;
        }

        public static ConfigHelper GetCurrentSettings(string key)
        {
            // Build configuration
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appSettings.json", false)
                .Build();
            var settings = new ConfigHelper(configuration.GetSection("AppSettings"), key);
            return settings;
        }
    }
}
