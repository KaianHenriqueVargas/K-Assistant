using System.Collections.Concurrent;
using System.Net.Sockets;

namespace Core.Cameras.Infraestrutura;

public sealed class CameraPortScanner(int[] ports)
{
    public async Task<List<int>> ScanAsync(string ip, int timeout)
    {
        var openPorts = new ConcurrentBag<int>();

        await Parallel.ForEachAsync(ports, async (port, _) =>
        {
            if (await IsPortOpenAsync(ip, port, timeout))
                openPorts.Add(port);
        });

        return openPorts.ToList();
    }

    private static async Task<bool> IsPortOpenAsync(string ip, int port, int timeout)
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