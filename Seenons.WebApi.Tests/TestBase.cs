using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests
{
    public class TestBase : IDisposable
    {
        protected Mock<IGetWasteStreamsUseCase> GetWasteStreamsUseCaseMock { get; } =
            new Mock<IGetWasteStreamsUseCase>();

        protected TestServer? Server { get; private set; }
        private Startup? Startup { get; set; }

        protected TestBase()
        {
        }

        private void RegisterMocksWithContainer()
        {
            Startup = new StartupTestable(ConfigurationBuilderOverrides)
            {
                RegisterOverrides = services =>
                {
                    services.AddSingleton(_ => GetWasteStreamsUseCaseMock.Object);
                }
            };
        }

        protected void StartServer()
        {
            RegisterMocksWithContainer();

            var builder = new WebHostBuilder()
                         .ConfigureServices(Startup!.ConfigureServices)
                         .Configure(Startup.Configure);
            Server = new TestServer(builder);
        }

        protected Task<HttpResponseMessage> ExecuteGetRequestAsync(string uri)
        {
            return Server!.CreateRequest(path: uri)
                          .GetAsync();
        }
      
        protected virtual void ConfigurationBuilderOverrides(IConfigurationBuilder builder)
        {
        }

        public virtual void Dispose() => Server?.Dispose();
    }
}
