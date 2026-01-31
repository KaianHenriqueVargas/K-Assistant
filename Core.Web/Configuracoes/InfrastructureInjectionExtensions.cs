using Core.Web.Infraestrutura.Network;
using Core.Web.Infraestrutura.Network.Abstracoes;
using Infraestrutura.Data;
using Lib.Servicos.InjecoesDeDependencia;

namespace Core.Web.Configuracoes;

public static class InfrastructureInjectionExtensions
{
    public static IServiceCollection InjetarInfraestrutura(this IServiceCollection services,
        IConfiguration configuracao)
    {
        var configuracoesDoBancoDeDados = configuracao.GetSection("DatabaseSettings");

        services.InjetarBaseDeDados<AppDbContext>(configuracoesDoBancoDeDados);
        
        services.AddSingleton<IServicoDeDeteccaoDeRedeLocal, ServicoDeDeteccaoDeRedeLocal>();
        return services;
    }
}