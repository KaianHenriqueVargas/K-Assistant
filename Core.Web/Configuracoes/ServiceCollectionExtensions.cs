namespace Core.Web.Configuracoes;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigurarServicos(
        this IServiceCollection services,
        IConfiguration configuracao)
    {
        services.AddControllers();
        services.AddHttpClient();

        services.InjetarAplicacao();       // seus services
        services.InjetarInfraestrutura(configuracao);  // network, db, etc
        services.InjetarDominio(configuracao); // cross-cutting (MediatR, AutoMapper...)

        return services;
    }
}