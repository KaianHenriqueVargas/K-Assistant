using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Core.Web.Infraestrutura.Network;

public static class NetworkScanner
{
    public static string ObterNetworkLocal()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(i =>
                i.OperationalStatus == OperationalStatus.Up &&
                i.NetworkInterfaceType != NetworkInterfaceType.Loopback);

        foreach (var iface in interfaces)
        {
            var props = iface.GetIPProperties();

            var unicast = props.UnicastAddresses
                .FirstOrDefault(a =>
                    a.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(a.Address));

            if (unicast is null)
                continue;

            var ip = unicast.Address;
            var mask = unicast.IPv4Mask;

            var network = CalcularNetwork(ip, mask);

            return $"{network}/24"; // padrão simples
        }

        throw new InvalidOperationException("Nenhuma rede local IPv4 encontrada.");
    }

    public static IReadOnlyCollection<string> GerarIps(string networkRange)
    {
        if (!networkRange.Contains('/'))
            return new[] { networkRange };

        var baseIp = networkRange.Split('/')[0];
        var parts = baseIp.Split('.');

        var prefix = $"{parts[0]}.{parts[1]}.{parts[2]}.";

        var ips = new List<string>();

        for (int i = 1; i < 255; i++)
            ips.Add(prefix + i);

        return ips;
    }

    private static string CalcularNetwork(IPAddress ip, IPAddress mask)
    {
        var ipBytes = ip.GetAddressBytes();
        var maskBytes = mask.GetAddressBytes();

        var networkBytes = new byte[4];

        for (int i = 0; i < 4; i++)
            networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);

        return string.Join('.', networkBytes);
    }
}