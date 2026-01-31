using Core.Web.Configuracoes;
using Lib.Servicos.Extensoes;
using Serilog;

namespace Core.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = InicializacaoExtensao
                .CriarWebApplicationBuilderCore()
                .ConfigurarBuilder();

            await builder.Build().ConfigurarApp().RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, e.Message);
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}