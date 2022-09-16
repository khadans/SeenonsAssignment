using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Seenons.Ports;

namespace Seenons.WasteStreams
{
    public interface IGetWasteStreamsUseCase
    {
        Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode, ushort[] days);
    }

    public class GetWasteStreamsUseCase : IGetWasteStreamsUseCase
    {
        private readonly ILogger<GetWasteStreamsUseCase> _logger;

        private readonly IWasteStreamsDataStore _wasteStreamsDataStore;

        public GetWasteStreamsUseCase(IWasteStreamsDataStore wasteStreamsDataStore, ILogger<GetWasteStreamsUseCase> logger)
        {
            _wasteStreamsDataStore = wasteStreamsDataStore;
            _logger = logger;
        }

        public async Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode, ushort[] days)
        {
            try
            {
                var wasteStreams = days != null && days.Any() ?
                                       _wasteStreamsDataStore.GetWasteStreamsAsync(postalCode, days) :
                                       _wasteStreamsDataStore.GetWasteStreamsAsync(postalCode);

                _logger.LogInformation($"Successfully retrieved waste streams for postal code {postalCode}.");

                return await wasteStreams;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while getting waste streams for postal code {postalCode}: {ex.Message}");
                throw;
            }
        }
    }
}