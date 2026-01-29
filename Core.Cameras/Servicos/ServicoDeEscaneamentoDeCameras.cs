using System.Collections.Concurrent;
using System.Diagnostics;
using Core.Cameras.Abstracoes;
using Core.Cameras.Dtos;
using Core.Cameras.Extensoes;
using Core.Cameras.Infraestrutura;

namespace Core.Cameras.Servicos;

public sealed class ServicoDeEscaneamentoDeCameras(IHttpClientFactory factory) : IServicoDeEscaneamentoDeCameras
{
    private readonly CameraPortScanner _portScanner = new(PortasPadrao.Cameras);
    private readonly CameraIdentifier _identifier = new(factory.CreateClient());

    private static void DefinirNetworkPadraoSeNecessario(NetworkScanRequest request)
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
        NetworkScanRequest request,
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

    public async Task<ResultadoEscaneamento> ScanNetworkAsync(NetworkScanRequest request)
    {
        DefinirNetworkPadraoSeNecessario(request);

        var ips = NetworkScanner.GerarIps(request.NetworkRange!);
        var cameras = new ConcurrentBag<Camera>();

        await Parallel.ForEachAsync(ips, new ParallelOptions
            {
                MaxDegreeOfParallelism = request.MaxThreads
            },
            async (ip, ct) =>
            {
                if (!await PingService.EstaOnlineAsync(ip, request.TimeoutMs))
                    return;

                var ports = await _portScanner.ScanAsync(ip, request.TimeoutMs);
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


    public async Task<Camera?> ScanSingleDeviceAsync(string ipAddress)
        => await EscanearIpAsync(ipAddress, timeoutMs: 2000, deepScan: true);

    public IReadOnlyCollection<int> GetCommonCameraPorts()
        => PortasPadrao.Cameras;

    // ======================
    // Métodos privados
    // ======================

    private async Task<Camera?> EscanearIpAsync(
        string ip,
        int timeoutMs,
        bool deepScan)
    {
        if (!await PingService.EstaOnlineAsync(ip, timeoutMs))
            return null;

        var ports = await _portScanner.ScanAsync(ip, timeoutMs);
        if (!ports.Any())
            return null;

        return deepScan
            ? await _identifier.IdentificarAsync(ip, ports)
            : new Camera
            {
                EnderecoDeIP = ip,
                Porta = ports.Min(),
                EstaOnline = true
            };
    }
}