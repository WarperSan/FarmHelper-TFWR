using System;
using System.Reflection;

namespace FarmHelper.API;

internal static class AssemblyLoader
{
    public static void LoadEmbeddedDLL()
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var resourceName = Resource.GetDLL(new AssemblyName(args.Name).Name);

            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(resourceName);
            
            if (stream == null)
                return null;
            
            var assemblyData = new byte[stream.Length];

            _ = stream.Read(assemblyData, 0, assemblyData.Length);

            return Assembly.Load(assemblyData);
        };
    }
}