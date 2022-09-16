using System;

namespace Seenons.Persistence.WasteStreams.Models
{
    public class TimeSlotRow
    {
        public int providerpickupareaid { get; set; }
        public short day { get; set; }
        public short weekrecurrence { get; set; }
        public TimeSpan pickupstart { get; set; }
        public string logisticalprovidername { get; set; }
    }
}
