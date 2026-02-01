namespace Core.Web.Models.Dtos;

public sealed class RtspProfile(string caminho)
{
    public string Caminho { get; } = caminho;

    public string MontarUrl(string usuario, string senha, string ip, int porta) =>
        $"rtsp://{usuario}:{senha}@{ip}:{porta}/{Caminho}";
}