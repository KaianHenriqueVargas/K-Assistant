namespace Core.Web.Configuracoes;

public static class HostExtensions
{
    public static void ConfigurarHost(
        this WebApplicationBuilder builder)
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    }
}