using Core.Web.Infraestrutura.Network;
using Core.Web.Infraestrutura.Network.Abstracoes;
using Infraestrutura.Data;
using Lib.Servicos.InjecoesDeDependencia;

namespace Core.Web.Configuracoes;

public static class InjecoesDeDependenciaDaInfraestrutura
{
    public static IServiceCollection InjetarInfraestrutura(this IServiceCollection services,
        IConfiguration configuracao)
    {
        services.InjetarBaseDeDados(configuracao);
        services.InjetarServicos();
        return services;
    }

    private static void InjetarBaseDeDados(this IServiceCollection services, IConfiguration configuracao)
    {
        var configuracoesDoBancoDeDados = configuracao.GetSection("DatabaseSettings");

        services.InjetarBaseDeDados<AppDbContext>(configuracoesDoBancoDeDados);
    }

    private static void InjetarServicos(this IServiceCollection services)
    {
        services.AddSingleton<IServicoDeDeteccaoDeRedeLocal, ServicoDeDeteccaoDeRedeLocal>();
    }
}