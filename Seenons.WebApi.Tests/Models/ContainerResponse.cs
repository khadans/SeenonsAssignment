using Seenons.WasteStreams;

namespace Seenons.WebApi.Tests.Models
{
    public class ContainerResponse
    {
        public ContainerResponse(int containerProductId, string type, string name)
        {
            ContainerProductId = containerProductId;
            Type = type;
            Name = name;
        }

        public int ContainerProductId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        public static ContainerResponse From(Container container)
        {
            return new ContainerResponse(container.Id, container.Type, container.Name);
        }
    }
}
