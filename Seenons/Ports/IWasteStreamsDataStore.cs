using System.Collections.Generic;
using System.Threading.Tasks;
using Seenons.WasteStreams;

namespace Seenons.Ports
{
    public interface IWasteStreamsDataStore
    {
        Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode);
        Task<IEnumerable<WasteStream>> GetWasteStreamsAsync(string postalCode, ushort[] days);
    }
}
