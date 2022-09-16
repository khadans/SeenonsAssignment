using Seenons.Adapters.Persistence;

namespace Seenons.WebApi
{
    public class Settings: IDbSettings
    {
        public string DbConnectionString { get; set; }
    }
}
