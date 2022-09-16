using System.Linq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class ProviderPickupAreaDayResponse
    {
        public Day Day { get; }
        public short WeekRecurrence { get; }
        public PickupIntervalResponse[] PickupIntervals { get; }
        public string Description =>
         WeekRecurrence == 1 ? $"Every {Day}" : $"Every {WeekRecurrence} Week on {Day}";

        public ProviderPickupAreaDayResponse(short day, short weekRecurrence, PickupIntervalResponse[] pickupIntervals)
        {
            Day = (Day)day;
            WeekRecurrence = weekRecurrence;
            PickupIntervals = pickupIntervals;
        }

        public static ProviderPickupAreaDayResponse From(ProviderPickupAreaDay providerPickupAreaDay) =>
            new ProviderPickupAreaDayResponse(providerPickupAreaDay.Day,
                                              providerPickupAreaDay.WeekRecurrence,
                                              providerPickupAreaDay.PickupTimes.Select(p => new PickupIntervalResponse(p)).ToArray());
    }
}
