using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Seenons.WasteStreams;
using Seenons.WebApi.Tests.Models;
using Xunit;

namespace Seenons.WebApi.Tests
{
    public class WasteStreamsControllerIntegrationTests: TestBase
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task Get_ByPostalCode_ReturnsOkWithResult()
        {
            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            var expectedWasteStreams = wasteStreams.Select(WasteStreamResponse.From).ToArray();

            var expectedResponse = new GetWasteStreamsResponse()
                                   {
                                       Items = expectedWasteStreams,
                                       TotalItems = expectedWasteStreams.Length
                                   };

            GetWasteStreamsUseCaseMock.Setup(
                                           u => u
                                              .GetWasteStreamsAsync(It.IsAny<string>(), It.IsAny<ushort[]>())
                                       )
                                      .ReturnsAsync(wasteStreams);

            var result = await ExecuteGetRequestAsync("api/wastestreams?postalCode=1000");

            using (new AssertionScope())
            {
                result.StatusCode.Should().Be(HttpStatusCode.OK);

                var response = (await result.Content.ReadAsAsync<GetWasteStreamsResponse>());

                response.Should().BeEquivalentTo(expectedResponse);
            }
        }

        [Fact]
        public async Task Get_ByPostalCodeAndDays_ReturnsOkWithResult()
        {
            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            var expectedWasteStreams = wasteStreams.Select(WasteStreamResponse.From).ToArray();

            var expectedResponse = new GetWasteStreamsResponse()
                                   {
                                       Items = expectedWasteStreams,
                                       TotalItems = expectedWasteStreams.Length
                                   };

            GetWasteStreamsUseCaseMock.Setup(
                                           u => u
                                              .GetWasteStreamsAsync(It.IsAny<string>(), new ushort[] { 1,2 })
                                       )
                                      .ReturnsAsync(wasteStreams);

            var result = await ExecuteGetRequestAsync("api/wastestreams?postalCode=1000&Weekdays=1&Weekdays=2");

            using (new AssertionScope())
            {
                result.StatusCode.Should().Be(HttpStatusCode.OK);

                var response = (await result.Content.ReadAsAsync<GetWasteStreamsResponse>());

                response.Should().BeEquivalentTo(expectedResponse);
            }
        }

        [Fact]
        public async Task Get_WithoutPostalCode_ReturnsBadRequest()
        {
            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            GetWasteStreamsUseCaseMock.Setup(
                                           u => u
                                              .GetWasteStreamsAsync(It.IsAny<string>(), It.IsAny<ushort[]>())
                                       )
                                      .ReturnsAsync(wasteStreams);

            var result = await ExecuteGetRequestAsync("api/wastestreams");
            
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_WithInvalidPostalCode_ReturnsBadRequest()
        {
            string invalidPostalCode = "invalidCode";

            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            GetWasteStreamsUseCaseMock.Setup(
                                           u => u
                                              .GetWasteStreamsAsync(It.IsAny<string>(), It.IsAny<ushort[]>())
                                       )
                                      .ReturnsAsync(wasteStreams);

            var result = await ExecuteGetRequestAsync($"api/wastestreams?postalCode={invalidPostalCode}");

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task Get_WithInvalidWeekdays_ReturnsBadRequest()
        {
            string invalidWeekDays = "invalidWeekDays";

            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            GetWasteStreamsUseCaseMock.Setup(
                                           u => u
                                              .GetWasteStreamsAsync(It.IsAny<string>(), new ushort[] { 1, 2 })
                                       )
                                      .ReturnsAsync(wasteStreams);

            var result = await ExecuteGetRequestAsync($"api/wastestreams?postalCode=1000&Weekdays={invalidWeekDays}");

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public WasteStreamsControllerIntegrationTests() => StartServer();
    }
}
