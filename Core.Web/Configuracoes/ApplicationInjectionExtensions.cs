using Core.Web.Aplicacao.Cameras.Abstracoes;
using Core.Web.Aplicacao.Cameras.Servicos;

namespace Core.Web.Configuracoes;

public static class ApplicationInjectionExtensions
{
    public static IServiceCollection InjetarAplicacao(
        this IServiceCollection services)
    {
        // Serviços de caso de uso / aplicação
        services.AddScoped<
            IServicoDeEscaneamentoDeCameras,
            ServicoDeEscaneamentoDeCameras>();

        // Se no futuro tiver outros serviços de aplicação:
        // services.AddScoped<IOutroServico, OutroServico>();

        return services;
    }
}