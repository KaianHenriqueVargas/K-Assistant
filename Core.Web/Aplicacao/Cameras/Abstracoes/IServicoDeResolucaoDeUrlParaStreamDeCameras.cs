namespace Core.Web.Aplicacao.Cameras.Abstracoes;

public interface IServicoDeResolucaoDeUrlParaStreamDeCameras
{
    bool Suporta(string fabricante, string protocolo);

    string MontarUrl(string ip, int porta, string usuario, string senha);
}