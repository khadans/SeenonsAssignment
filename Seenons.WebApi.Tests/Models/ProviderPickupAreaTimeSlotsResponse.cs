using System.Linq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests.Models
{
    public class ProviderPickupAreaTimeSlotsResponse
    {
        public int ProviderPickupAreaId { get; set; }
        public string LogisticalProviderName { get; set; }
        public ProviderPickupAreaDayResponse[] Days { get; set; }

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
