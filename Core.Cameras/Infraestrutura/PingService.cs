using System.Net.NetworkInformation;

namespace Core.Cameras.Infraestrutura;

public static class PingService
{
    public static async Task<bool> EstaOnlineAsync(string ip, int timeout)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, timeout);
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}