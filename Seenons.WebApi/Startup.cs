using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Seenons.Adapters.Persistence;
using Seenons.Persistence;
using Seenons.Ports;
using Seenons.WasteStreams;
using Seenons.WebApi.Infrastructure;
using Serilog;

namespace Seenons.WebApi
{
    public class Startup
    {
        private readonly Settings _settings;

        public Action<IServiceCollection>? RegisterOverrides { get; set; }

        public Startup(IConfiguration configuration)
        {
            _settings = new Settings();

            configuration.Bind(_settings);

            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .WriteTo.Console()
                        .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Warning)
                                                                .AddConsole());
            services.AddControllers(
                         opt => { opt.OutputFormatters.RemoveType<StringOutputFormatter>(); }
                     )
                    .AddJsonOptions(
                         options => { options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter()); }
                     );

            services.AddSwaggerGen();
            services.AddSingleton<GlobalExceptionLogger>();
            services.AddSingleton(_settings);
            services.AddSingleton((IDbSettings)_settings);

            services.AddTransient<IGetWasteStreamsUseCase, GetWasteStreamsUseCase>();
            services.AddTransient<IWasteStreamsDataStore, WasteStreamsDataStore>();
            RegisterOverrides?.Invoke(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            ConfigureMiddleware(app);
        }

        private void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(
                new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All,
                }
            );

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1"));

            app.UseRouting();

            app.UseEndpoints(
                endpoints => endpoints.MapControllers()
            );
        }
    }
}
