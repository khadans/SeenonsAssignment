using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Seenons.WebApi.Tests
{
    public class StartupTestable : Startup
    {
        private static IConfiguration CreateConfiguration(Action<IConfigurationBuilder>? overrides)
        {
            var configurationSettings =
                new Dictionary<string, string>
                {
                    { "OracleConnectionStringTemplate", "Data Source=TEST" },
                    { "OraclePassword", "fake" },
                    { "OraclePasswordTemplate", "fake" },
                    { "OpenIdConnectAuthorityUri", "http://www.coolblue.nl/" },
                    { "OracleResilience:RetryCount", "3" },
                    { "OracleResilience:RetryDelayMilliseconds", "250" },
                    { "OracleResilience:RetrySeedDelayMilliseconds", "1" },
                    { "OracleResilience:CircuitBreakerAllowedAttempts", "10" },
                    { "OracleResilience:CircuitBreakerDurationMilliseconds", "100" },
                };

            var builder = new ConfigurationBuilder()
               .AddInMemoryCollection(configurationSettings);

            overrides?.Invoke(builder);

            IConfigurationRoot configuration = builder.Build();

            return configuration;
        }

        public StartupTestable(Action<IConfigurationBuilder>? configurationOverrides = null)
            : base(CreateConfiguration(configurationOverrides))
        {
        }
    }
}