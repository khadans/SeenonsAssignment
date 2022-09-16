using Seenons.WasteStreams;

namespace Seenons.WebApi.Models.WasteStreams
{
    public class ContainerResponse
    {
        public ContainerResponse(int containerProductId, string type, string name)
        {
            ContainerProductId = containerProductId;
            Type = type;
            Name = name;
        }

        public int ContainerProductId { get; }
        public string Type { get; }
        public string Name { get; }

        public static ContainerResponse From(Container container)
        {
            return new ContainerResponse(container.Id, container.Type, container.Name);
        }
    }
}
