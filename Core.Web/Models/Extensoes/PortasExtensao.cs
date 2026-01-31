namespace Core.Web.Models.Extensoes;

public static class PortasExtensao
{
    public static bool IsHttp(this int port) => port is 80 or 8080;

    public static bool IsHttps(this int port) => port is 443;

    public static bool IsRtsp(this int port) => port is 554 or 8554;
}