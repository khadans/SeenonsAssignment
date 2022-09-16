using System;

namespace Seenons.WasteStreams
{
    public class ProviderPickupAreaTimeSlots
    {
        public int ProviderPickupAreaId { get; set; }
        public string LogisticalProviderName { get; set; }
        public ProviderPickupAreaDay[] Days { get; set; }
    }
    public class ProviderPickupAreaDay
    {
        public short Day { get; set; }
        public short WeekRecurrence { get; set; }
        public TimeSpan[] PickupTimes { get; set; }
    }
}
