using System.Net.Sockets;
using System.Text.RegularExpressions;
using Core.Web.Infraestrutura.Http;
using Core.Web.Models.Enumerados;
using Core.Web.Models.Extensoes;

namespace Core.Web.Infraestrutura.Camera;

public sealed class IdentificadorDeCamera(HttpClient httpClient)
{
    private static readonly Dictionary<FabricanteCamera, string[]> AssinaturasHttp = new()
    {
        { FabricanteCamera.Dahua,     ["Dahua", "DVR"] },
        { FabricanteCamera.Hikvision, ["Hikvision", "HIK"] },
        { FabricanteCamera.TpLink,    ["TP-LINK", "Tapo"] },
        { FabricanteCamera.Axis,      ["AXIS"] },
        { FabricanteCamera.Reolink,   ["Reolink"] },
        { FabricanteCamera.Ezviz,     ["EZVIZ"] },
        { FabricanteCamera.Xiaomi,    ["Xiaomi"] }
    };

    public async Task<Models.Entidades.Camera?> IdentificarAsync(string ip, IEnumerable<int> ports)
    {
        var portasOrdenadas = ports.OrderBy(p => p).ToList();

        var rtspPort = portasOrdenadas.FirstOrDefault(p => p.IsRtsp());
        
        if (rtspPort > 0)
        {
            var fabricanteRtsp = IdentificarFabricantePorRtspHeuristica(rtspPort);
            
            return CriarCamera(
                ip,
                rtspPort,
                fabricanteRtsp,
                ProtocoloCamera.Rtsp);
        }

        foreach (var port in portasOrdenadas.Where(p => p.IsHttp() || p.IsHttps()))
        {
            var cameraHttp = await IdentificarViaHttpAsync(ip, port);
            if (cameraHttp is not null)
                return cameraHttp;
        }

        return null;
    }
    private async Task<Models.Entidades.Camera?> IdentificarViaHttpAsync(string ip, int port)
    {
        var protocolo = port.IsHttps()
            ? ProtocoloCamera.Https
            : ProtocoloCamera.Http;

        var url = $"{protocolo.ToString().ToLower()}://{ip}:{port}";

        try
        {
            using var response = await httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            foreach (var (fabricante, assinaturas) in AssinaturasHttp)
            {
                if (assinaturas.Any(sig => response.ContemAssinatura(body, sig)))
                {
                    return CriarCamera(ip, port, fabricante, protocolo);
                }
            }
        }
        catch
        {
            // Ignora falhas individuais
        }

        return null;
    }
    private static FabricanteCamera IdentificarFabricantePorRtspHeuristica(int port)
    {
        return port switch
        {
            554   => FabricanteCamera.Hikvision,
            8554  => FabricanteCamera.Dahua,
            10554 => FabricanteCamera.Axis,
            _     => FabricanteCamera.Desconhecido
        };
    }
    
    private static async Task<string?> TentarExtrairModeloViaSdpAsync(string ip, int port)
    {
        try
        {
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(ip, port);

            using var stream = tcp.GetStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };
            using var reader = new StreamReader(stream);

            await writer.WriteLineAsync(
                $"DESCRIBE rtsp://{ip}:{port}/ RTSP/1.0\r\nCSeq: 1\r\n\r\n");

            var resposta = await reader.ReadToEndAsync();

            var match = Regex.Match(
                resposta,
                @"(Hikvision|Dahua|AXIS|DS-\w+|IPC-\w+)",
                RegexOptions.IgnoreCase);

            return match.Success ? match.Value : null;
        }
        catch
        {
            return null;
        }
    }
    
    private static Models.Entidades.Camera CriarCamera(
        string ip,
        int port,
        FabricanteCamera fabricante,
        ProtocoloCamera protocolo)
        => new()
        {
            EnderecoDeIP = ip,
            Porta = port,
            Fabricante = fabricante.ToString(),
            Protocolo = protocolo.ToString(),
            EstaOnline = true
        };
}
