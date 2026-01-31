using Core.Web.Aplicacao.Cameras.Abstracoes;
using Core.Web.Aplicacao.Cameras.Servicos;

namespace Core.Web.Configuracoes;

public static class InjecoesDeDependenciaDaAplicacao
{
    public static IServiceCollection InjetarAplicacao(
        this IServiceCollection services)
    {
        services.AddScoped<IServicoDeEscaneamentoDeCameras, ServicoDeEscaneamentoDeCameras>();

        return services;
    }
}