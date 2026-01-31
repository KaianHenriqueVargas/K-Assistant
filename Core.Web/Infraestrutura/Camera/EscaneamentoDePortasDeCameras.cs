using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Core.Web.Infraestrutura.Camera;

public sealed class EscaneamentoDePortasDeCameras(int[] ports)
{
    public async Task<List<int>> EscanearAsync(string ip, int timeout)
    {
        var openPorts = new ConcurrentBag<int>();

        await Parallel.ForEachAsync(ports, async (port, _) =>
        {
            if (await PortaEstaAbertaAsync(ip, port, timeout))
                openPorts.Add(port);
        });

        return openPorts.ToList();
    }

    private static async Task<bool> PortaEstaAbertaAsync(string ip, int port, int timeout)
    {
        try
        {
            using var client = new TcpClient();
            using var cts = new CancellationTokenSource(timeout);
            await client.ConnectAsync(ip, port, cts.Token);
            return client.Connected;
        }
        catch
        {
            return false;
        }
    }
}