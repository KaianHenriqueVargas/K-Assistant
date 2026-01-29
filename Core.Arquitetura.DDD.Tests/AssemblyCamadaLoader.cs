using System.Reflection;

namespace Core.Arquitetura.DDD.Tests;

internal static class AssemblyCamadaLoader
{
    public static IReadOnlyList<Assembly> Carregar(string prefixo)
    {
        var prefixoNormalizado = prefixo.TrimEnd('.');

        return Directory
            .GetFiles(AppContext.BaseDirectory, $"{prefixoNormalizado}*.dll")
            .Select(Assembly.LoadFrom)
            .ToList();
    }
}