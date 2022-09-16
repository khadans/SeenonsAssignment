using System.Collections.Generic;

namespace Seenons.WebApi.Tests.Models
{
    public class GetWasteStreamsResponse
    {
        public IEnumerable<WasteStreamResponse> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
