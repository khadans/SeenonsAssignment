using Seenons.WasteStreams;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class WasteStreamSizeResponse
    {
        public int Id { get; }
        public short Size { get; }
        public ContainerResponse Container { get; }

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
