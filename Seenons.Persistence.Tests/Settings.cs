using System;
using Microsoft.Extensions.Configuration;
using Seenons.Adapters.Persistence;

namespace Seenons.Persistence.Tests
{
    public class Settings: IDbSettings
    {
        public static Settings Instance { get; }

        public string DbConnectionString { get; set; }

        static Settings()
        {
            var configuration = new ConfigurationBuilder()
                               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                               .Build();

            var configurationSection = configuration.GetSection("TestSettings");

            Instance = configurationSection.Get<Settings>();
        }
    }
}
