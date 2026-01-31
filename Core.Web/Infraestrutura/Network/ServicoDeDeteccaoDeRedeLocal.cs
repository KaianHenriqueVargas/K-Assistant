using System.Net;
using System.Net.NetworkInformation;
using Core.Web.Infraestrutura.Network.Abstracoes;

namespace Core.Web.Infraestrutura.Network;

public sealed class ServicoDeDeteccaoDeRedeLocal : IServicoDeDeteccaoDeRedeLocal
{
    public string ObterNetworkRange()
    {
        var iface = NetworkInterface.GetAllNetworkInterfaces()
            .FirstOrDefault(i =>
                i.OperationalStatus == OperationalStatus.Up &&
                i.NetworkInterfaceType != NetworkInterfaceType.Loopback);

        if (iface is null)
            throw new InvalidOperationException("Nenhuma interface de rede ativa encontrada.");

        var unicast = iface
            .GetIPProperties()
            .UnicastAddresses
            .First(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

        var ip = unicast.Address;
        var mask = unicast.IPv4Mask;

        var cidr = CalcularCidr(mask);
        return $"{ip}/{cidr}";
    }

    private static int CalcularCidr(IPAddress mask)
    {
        var bytes = mask.GetAddressBytes();
        var bits = bytes.Sum(b => Convert.ToString(b, 2).Count(c => c == '1'));
        return bits;
    }
}
