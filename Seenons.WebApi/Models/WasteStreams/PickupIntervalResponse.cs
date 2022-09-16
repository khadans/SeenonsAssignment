using System;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class PickupIntervalResponse
    {
        private const int HourInterval = 2;
        public TimeSpan PickupStartTime { get; }
        public TimeSpan PickupEndTime { get; }

        public PickupIntervalResponse(TimeSpan pickupStartTime)
        {
            PickupStartTime = pickupStartTime;
            PickupEndTime = pickupStartTime.Add(TimeSpan.FromHours(HourInterval));
        }
    }
}
