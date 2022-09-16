using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Seenons.WasteStreams;
using Seenons.WebApi.Controllers;
using Seenons.WebApi.Models;
using Seenons.WebApi.Models.WasteStreams;
using Xunit;

namespace Seenons.WebApi.Tests
{
    public class WasteStreamsControllerUnitTests
    {
        private readonly Mock<IGetWasteStreamsUseCase> _getWasteStreamsUseCaseMock = new Mock<IGetWasteStreamsUseCase>();
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task GetWasteStreamsAsync_WhenUseCaseReturnsException_ReturnsInternalServerErrorWithMessage()
        {
            var exception = new Exception("Exception message");

            _getWasteStreamsUseCaseMock.Setup(u => u.GetWasteStreamsAsync(It.IsAny<string>(), It.IsAny<ushort[]>())).ThrowsAsync(exception);

            var result = await CreateSut().GetWasteStreams(_fixture.Create<GetWasteStreamsRequest>());

            using (new AssertionScope())
            {
                var apiResult = result.Should().BeOfType<ObjectResult>().Which;
                apiResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                apiResult.Value.Should().BeOfType<ErrorResponse>()
                         .Which.Message.Should().Contain(exception.Message);
            }
        }

        [Fact]
        public async Task GetWasteStreamsAsync_WhenUseCaseSucceeds_ReturnsOkWithContent()
        {
            var wasteStreams = new[] { _fixture.Create<WasteStream>(), _fixture.Create<WasteStream>() };

            var expectedResponse = wasteStreams.Select(WasteStreamResponse.From);

            _getWasteStreamsUseCaseMock.Setup(u => u.GetWasteStreamsAsync(It.IsAny<string>(), It.IsAny<ushort[]>())).ReturnsAsync(wasteStreams);

            var result = await CreateSut().GetWasteStreams(_fixture.Create<GetWasteStreamsRequest>());

            using (new AssertionScope())
            {
                var value = result.Should().BeOfType<OkObjectResult>().Which.Value;
                value.Should().BeEquivalentTo(
                    new
                    {
                        items = expectedResponse,
                        totalItems = 2
                    }
                );
            }
        }

        private WasteStreamsController CreateSut()
        {
            return new WasteStreamsController(_getWasteStreamsUseCaseMock.Object);
        }
    }
}
