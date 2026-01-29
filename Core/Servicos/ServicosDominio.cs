using System.Globalization;
using System.Reflection;
using Core.Infraestrutura.Data;
using Lib.Infraestrutura.Configuracoes;
using Lib.Infraestrutura.Cultura;
using Lib.Servicos.InjecoesDeDependencia;
using Servicos.InjecoesDeDependencia;

namespace Core.Servicos;

public static class ServicosDominio
{
    public static IServiceCollection InjetarDominio(
        this IServiceCollection services, IConfiguration configuracao, ConfiguracaoInjecao? configuracaoInjecao = null)
    {
        configuracaoInjecao ??= new ConfiguracaoInjecao();
        CultureInfo.DefaultThreadCurrentCulture = Cultura.Brasil;
        var assemblies = ObterAssembliesVerificandoDuplicações(configuracao);
        var configuracoesDoBancoDeDados = configuracao.GetSection("DatabaseSettings");

        services.InjetarBaseDeDados<AppDbContext>(configuracoesDoBancoDeDados);
        services.InjetarAutoMapper(assemblies, configuracaoInjecao);
        services.InjetarServicosGerais(configuracaoInjecao);
        services.InjetarRepositorios(assemblies);
        services.InjetarMediatR(assemblies);

        Host.CreateDefaultBuilder().ConfigureServices((serviceCollection) =>
        {
            serviceCollection.InjetarRequisicaoContexto();
        });
        services.InjetarRequisicaoContexto();
        services.InjetarValidador();
        services.InjetarValidadorDominio();
        return services;
    }

    private static Assembly[] ObterAssembliesVerificandoDuplicações(IConfiguration configuracao)
    {
        var assemblies = configuracao
            .ObterConfiguracao<InjecaoDominio>()
            ?.Assemblys
            .Select(Assembly.Load).Distinct().ToArray();
        if (assemblies == null) return [];
        assemblies.ValidarAssemblies();
        return assemblies;
    }

    private static T? ObterConfiguracao<T>(this IConfiguration configuracao) =>
        configuracao.GetSection(typeof(T).Name).Get<T>();

    private static void ValidarAssemblies(this Assembly[] assemblies)
    {
        var assembliesDuplicados = assemblies.ObterSeExisteAlgumAssemblyDuplicado();
        if (!string.IsNullOrEmpty(assembliesDuplicados))
            throw new ArgumentException($"Um ou mais 'assemblies' estão duplicados: '{assembliesDuplicados}'");
    }

    private static string ObterSeExisteAlgumAssemblyDuplicado(this Assembly[] assemblies) =>
        assemblies.GroupBy(a => a.FullName)
            .Select(group => new
            {
                Nome = group.Key,
                Qtd = group.Count()
            })
            .Where(x => x.Qtd > 1) is { } duplicados
            ? string.Join(",", duplicados)
            : string.Empty;
}