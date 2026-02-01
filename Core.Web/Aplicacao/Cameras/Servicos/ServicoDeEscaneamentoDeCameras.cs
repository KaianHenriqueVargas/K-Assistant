using System.Collections.Concurrent;
using Core.Web.Aplicacao.Cameras.Abstracoes;
using Core.Web.Aplicacao.Cameras.Comandos;
using Core.Web.Aplicacao.Cameras.Dtos;
using Core.Web.Infraestrutura.Camera;
using Core.Web.Infraestrutura.Network;
using Core.Web.Models.Constantes;
using Core.Web.Models.Entidades;

namespace Core.Web.Aplicacao.Cameras.Servicos;

public sealed class ServicoDeEscaneamentoDeCameras(IHttpClientFactory factory) : IServicoDeEscaneamentoDeCameras
{
    private readonly EscaneamentoDePortasDeCameras _escaneamentoDePortas = new(Portas.Cameras);
    private readonly IdentificadorDeCamera _identifier = new(factory.CreateClient());

    private static void DefinirNetworkPadraoSeNecessario(EscanearRedeComando request)
    {
        if (string.IsNullOrWhiteSpace(request.NetworkRange))
            request.NetworkRange = NetworkScanner.ObterNetworkLocal();
    }

    private static Camera CriarCameraBasica(string ip, IEnumerable<int> ports)
    {
        return new Camera
        {
            EnderecoDeIP = ip,
            Porta = ports.Min(),
            EstaOnline = true
        };
    }

    private static ResultadoEscaneamento CriarResultado(
        EscanearRedeComando request,
        int totalIps,
        IEnumerable<Camera> cameras)
    {
        return new ResultadoEscaneamento
        {
            NetworkRange = request.NetworkRange!,
            TotalDispositivosEscaneados = totalIps,
            Cameras = cameras
                .OrderBy(c => c.EnderecoDeIP)
                .ToList()
        };
    }

    public async Task<ResultadoEscaneamento> ScanNetworkAsync(EscanearRedeComando request)
    {
        DefinirNetworkPadraoSeNecessario(request);

        var ips = NetworkScanner.GerarIps(request.NetworkRange!);
        var cameras = new ConcurrentBag<Camera>();

        await Parallel.ForEachAsync(ips, new ParallelOptions
            {
                MaxDegreeOfParallelism = request.NumeroMaximoDeThreads
            },
            async (ip, ct) =>
            {
                if (!await ServicoDePing.EstaOnlineAsync(ip, request.Timeout))
                    return;

                var ports = await _escaneamentoDePortas.EscanearAsync(ip, request.Timeout);
                if (!ports.Any())
                    return;

                var camera = request.DeepScan
                    ? await _identifier.IdentificarAsync(ip, ports)
                    : CriarCameraBasica(ip, ports);

                if (camera is not null)
                    cameras.Add(camera);
            });

        return CriarResultado(request, ips.Count, cameras);
    }
}