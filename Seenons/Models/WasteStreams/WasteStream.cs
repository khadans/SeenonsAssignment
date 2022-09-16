using System.Collections.Generic;

namespace Seenons.WasteStreams
{
    public class WasteStream
    {
        //I'm leaving the public setters in order to be able to generate entities in tests for simplicity
        //(in real life there would be a constructor and more work to set up data in tests)

        public int StreamProductId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public IEnumerable<WasteStreamSize> Sizes { get; set; }

        public IEnumerable<ProviderPickupAreaTimeSlots> ProviderPickupAreaTimeSlots { get; set; }
    }
}
