using System;
using System.Linq;
using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests.Models
{
    public class ProviderPickupAreaDayResponse
    {
        public Day Day { get; set; }
        public short WeekRecurrence { get; set; }
        public PickupIntervalResponse[] PickupIntervals { get; set; }
        public string Description =>
         WeekRecurrence == 1 ? $"Every {Day}" : $"Every {WeekRecurrence} Week on {Day}";

        public ProviderPickupAreaDayResponse(short day, short weekRecurrence, PickupIntervalResponse[] pickupIntervals)
        {
            Day = (Day)day;
            WeekRecurrence = weekRecurrence;
            PickupIntervals = pickupIntervals;
        }

        public static ProviderPickupAreaDayResponse From(ProviderPickupAreaDay providerPickupAreaDay) =>
            new ProviderPickupAreaDayResponse(
                providerPickupAreaDay.Day,
                providerPickupAreaDay.WeekRecurrence,
                providerPickupAreaDay.PickupTimes.Select(
                    p => new PickupIntervalResponse
                         {
                             PickupStartTime = p.ToString(@"hh\:mm\:ss"),
                             PickupEndTime = p.Add(TimeSpan.FromHours(2)).ToString(@"hh\:mm\:ss")
                         }
                ).ToArray()
            );
    }
}
