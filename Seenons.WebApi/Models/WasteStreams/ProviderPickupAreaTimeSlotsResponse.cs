using System.Linq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class ProviderPickupAreaTimeSlotsResponse
    {
        public int ProviderPickupAreaId { get; }
        public string LogisticalProviderName { get; }
        public ProviderPickupAreaDayResponse[] Days { get; }

        public ProviderPickupAreaTimeSlotsResponse(int providerPickupAreaId,
                                                   string logisticalProviderName,
                                                   ProviderPickupAreaDayResponse[] days)
        {
            ProviderPickupAreaId = providerPickupAreaId;
            LogisticalProviderName = logisticalProviderName;
            Days = days;
        }

        public static ProviderPickupAreaTimeSlotsResponse From(ProviderPickupAreaTimeSlots providerPickupAreaTimeSlots) =>
            new ProviderPickupAreaTimeSlotsResponse(providerPickupAreaTimeSlots.ProviderPickupAreaId,
                                                    providerPickupAreaTimeSlots.LogisticalProviderName,
                                                    providerPickupAreaTimeSlots.Days.Select(ProviderPickupAreaDayResponse.From).ToArray());
    }
}
