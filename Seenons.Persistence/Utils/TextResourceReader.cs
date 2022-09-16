using System;
using System.IO;
using System.Reflection;

namespace Seenons.Persistence.Utils
{
    public interface IResourceReader<out T>
    {
        T Read(string path);
    }

    public class TextResourceReader : IResourceReader<string>
    {
        private readonly Assembly _assembly;
        private readonly string _basePath;

        public TextResourceReader(Type typeFromTargetAssembly, bool useAssemblyNameAsBasePath = true)
            : this(typeFromTargetAssembly, string.Empty, useAssemblyNameAsBasePath)
        {
        }

        public TextResourceReader(Type typeFromTargetAssembly, string basePath, bool useAssemblyNameAsBasePath = true)
            : this(typeFromTargetAssembly.GetTypeInfo().Assembly, basePath, useAssemblyNameAsBasePath)
        {
        }

        public TextResourceReader(Assembly targetAssembly, bool useAssemblyNameAsBasePath = true)
            : this(targetAssembly, string.Empty, useAssemblyNameAsBasePath)
        {
        }

        public TextResourceReader(Assembly targetAssembly, string basePath, bool useAssemblyNameAsBasePath = true)
        {
            _assembly = targetAssembly;
            _basePath = FileToResourcePath(
                Path.Combine(
                    (useAssemblyNameAsBasePath
                         ? NormalizeAssemblyPath(_assembly.GetName().Name)
                         : string.Empty),
                    basePath
                )
            );
        }

        public string Read(string path)
        {
            string result = null;
            string completeResourceName = FileToResourcePath(Path.Combine(_basePath, path ?? ""));

            using Stream stream = _assembly.GetManifestResourceStream(completeResourceName);
            if (stream != null)
            {
                var resourceStream = new StreamReader(stream);
                result = resourceStream.ReadToEnd();
            }

            return result;
        }

        private static string NormalizeAssemblyPath(string path) =>
            path?.Replace("-", "_");

        private static string FileToResourcePath(string path) =>
            path?.Replace('/', '.')
                 .Replace('\\', '.')
                 .Trim('.');
    }
}