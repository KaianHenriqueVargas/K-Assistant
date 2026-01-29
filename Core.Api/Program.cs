using Core.Dominio.Cameras.Abstracoes;
using Core.Dominio.Cameras.Dtos;
using Core.Dominio.Cameras.Servicos;
using Core.Servicos;
using Lib.Servicos.Extensoes;
using Serilog;

namespace MeEntregaAi.API;

public class Program
{
    public static async Task Main(string[] args) => await ExecutarApi();

    private static Task ExecutarApi(CancellationToken token = default)
    {
        try
        {
            var webAplicationBuilder = InicializacaoExtensao.CriarWebApplicationBuilderCore();

            var configuracao = webAplicationBuilder.Configuration;
            webAplicationBuilder.Services.AddHttpClient();
            webAplicationBuilder.Services.AddScoped<IServicoDeEscaneamentoDeCameras, ServicoDeEscaneamentoDeCameras>();
            webAplicationBuilder.Services.AddSingleton<IDetectorDeRedeLocal, DetectorDeRedeLocal>();

            webAplicationBuilder.Services.InjetarDominio(configuracao);
            
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            webAplicationBuilder.WebHost.UseUrls($"http://0.0.0.0:{port}");

            
            return webAplicationBuilder
                .Build()
                .ConfigurarApp()
                .RunAsync(token);
        }
        catch (Exception e)
        {
            Log.Logger
                .Fatal(e, e.Message);
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}