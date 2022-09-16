using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests.Models
{
    public class WasteStreamSizeResponse
    {
        public int Id { get; set; }
        public short Size { get; set; }
        public ContainerResponse Container { get; set; }

        public WasteStreamSizeResponse(int id, short size, ContainerResponse container)
        {
            Id = id;
            Size = size;
            Container = container;
        }

        public static WasteStreamSizeResponse From(WasteStreamSize wasteStreamSize) =>
            new WasteStreamSizeResponse(wasteStreamSize.Id,
                                        wasteStreamSize.Size,
                                        ContainerResponse.From(wasteStreamSize.Container));
    }
}
