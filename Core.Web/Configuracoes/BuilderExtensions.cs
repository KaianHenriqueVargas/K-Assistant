namespace Core.Web.Configuracoes;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigurarBuilder(
        this WebApplicationBuilder builder)
    {
        builder.Services.ConfigurarServicos(builder.Configuration);
        builder.ConfigurarHost();

        return builder;
    }
}