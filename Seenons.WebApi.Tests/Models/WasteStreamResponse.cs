using System.Collections.Generic;
using System.Linq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests.Models
{
    public class WasteStreamResponse
    {
        public int StreamProductId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public IEnumerable<WasteStreamSizeResponse> Sizes { get; set; }

        public IEnumerable<ProviderPickupAreaTimeSlotsResponse> ProviderPickupAreaTimeSlots { get; set; }

        public WasteStreamResponse(int streamProductId,
                                   string type,
                                   string name,
                                   IEnumerable<WasteStreamSizeResponse> sizes,
                                   IEnumerable<ProviderPickupAreaTimeSlotsResponse> providerPickupAreaTimeSlots)
        {
            StreamProductId = streamProductId;
            Type = type;
            Name = name;
            Sizes = sizes;
            ProviderPickupAreaTimeSlots = providerPickupAreaTimeSlots;
        }

        public static WasteStreamResponse From(WasteStream wasteStream)
        {
            return new WasteStreamResponse(
                wasteStream.StreamProductId,
                wasteStream.Type,
                wasteStream.Name,
                wasteStream.Sizes.Select(WasteStreamSizeResponse.From),
                wasteStream.ProviderPickupAreaTimeSlots.Select(ProviderPickupAreaTimeSlotsResponse.From)
            );
        }
    }
}
