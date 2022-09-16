using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Seenons.Ports;
using Seenons.WasteStreams;
using Xunit;

namespace Seenons.Tests
{
    public class GetWasteStreamsUseCaseUnitTests
    {
        private readonly Mock<IWasteStreamsDataStore> _wasteStreamsDataStoreMock = new Mock<IWasteStreamsDataStore>();
        private readonly Mock<ILogger<GetWasteStreamsUseCase>> _loggerMock = new Mock<ILogger<GetWasteStreamsUseCase>>();

        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task GetWasteStreamsAsync_WhenJustPostalCodeIsProvided_ReturnsWasteStreamsFromRepository()
        {
            var wasteStreams = _fixture.CreateMany<WasteStream>().ToArray();

            string postalCode = "1000";

            _wasteStreamsDataStoreMock.Setup(d => d.GetWasteStreamsAsync(postalCode)).ReturnsAsync(wasteStreams);

            var result = await CreateSut().GetWasteStreamsAsync(postalCode, null);

            using (new AssertionScope())
            {
                _wasteStreamsDataStoreMock.Verify(m => m.GetWasteStreamsAsync(postalCode));
                result.Should().BeEquivalentTo(wasteStreams);
            }
        }
        
        [Fact]
        public async Task GetWasteStreamsAsync_WhenPostalCodeAndDaysAreProvided_ReturnsWasteStreamsFromRepository()
        {
            var wasteStreams = _fixture.CreateMany<WasteStream>().ToArray();
            
            string postalCode = "1000";

            ushort[] days = { 1, 2 };

            _wasteStreamsDataStoreMock.Setup(d => d.GetWasteStreamsAsync(postalCode, days)).ReturnsAsync(wasteStreams);

            var result = await CreateSut().GetWasteStreamsAsync("1000", days);

            using (new AssertionScope())
            {
                _wasteStreamsDataStoreMock.Verify(m => m.GetWasteStreamsAsync(postalCode, days));
                result.Should().BeEquivalentTo(wasteStreams);
            }
        }

        [Fact]
        public async Task GetWasteStreamsAsync_WhenRepositoryThrowsException_RethrowsException()
        {
            string postalCode = "1000";

            ushort[] days = { 1, 2 };

            _wasteStreamsDataStoreMock.Setup(d => d.GetWasteStreamsAsync(postalCode, days))
                                      .ThrowsAsync(new Exception());

            Func<Task> call = async () => await CreateSut().GetWasteStreamsAsync(postalCode, days);

            await call.Should().ThrowExactlyAsync<Exception>();
        }
        private IGetWasteStreamsUseCase CreateSut() => new GetWasteStreamsUseCase(_wasteStreamsDataStoreMock.Object, _loggerMock.Object);
    }
}
