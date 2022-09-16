using System.Reflection;

namespace Seenons.Persistence.Utils
{
    public static class EmbeddedResource
    {
        public static IResourceReader<string> Reader = new TextResourceReader(
            Assembly.GetExecutingAssembly(),
            "Scripts",
            useAssemblyNameAsBasePath: true
        );
        public static string Get(string path) => Reader.Read(path);
    }
}
