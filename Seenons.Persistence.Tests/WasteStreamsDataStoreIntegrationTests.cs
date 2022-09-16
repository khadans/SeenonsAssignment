using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Seenons.Ports;
using Xunit;

namespace Seenons.Persistence.Tests
{
    //Simplified integration tests, that rely on the data added by seed method on db setup.
    //In the real life the test data would be generated in the tests and assertions would be done also for the details of the returned data.
    
    public class WasteStreamsDataStoreIntegrationTests
    {
        [Fact]
        public async Task GetWasteStreamsAsync_ForPostalCode_WhenNoPickupTimeSlotsAvailable_ReturnsEmptyCollection()
        {
            var res = await CreateSut().GetWasteStreamsAsync("9999");
            res.Should().BeEmpty();
        }

        [Fact]
        public async Task GetWasteStreamsAsync_ForPostalCodeAndDays_WhenNoPickupTimeSlotsAvailable_ReturnsEmptyCollection()
        {
            var res = await CreateSut().GetWasteStreamsAsync("9999", new ushort[]{ 1 });
            res.Should().BeEmpty();
        }

        [Fact]
        public async Task GetWasteStreamsAsync_ForPostalCode_WhenPickupTimeSlotsAvailable_ReturnsCollection()
        {
            var res = await CreateSut().GetWasteStreamsAsync("1000");
            res.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetWasteStreamsAsync_ForPostalCodeAndDays_WhenPickupTimeSlotsAvailable_ReturnsCollection()
        {
            var res = (await CreateSut().GetWasteStreamsAsync("1000", new ushort[] { 1 })).ToArray();

            using (new AssertionScope())
            {
                res.Should().NotBeEmpty();
                var days = res.SelectMany(r => r.ProviderPickupAreaTimeSlots
                                                .SelectMany(t => t.Days))
                              .Select(d => d.Day);
                
                days.Should().AllBeEquivalentTo(1);
            }
        }

        private IWasteStreamsDataStore CreateSut()
        {
            return new WasteStreamsDataStore(Settings.Instance);
        }
    }
}
