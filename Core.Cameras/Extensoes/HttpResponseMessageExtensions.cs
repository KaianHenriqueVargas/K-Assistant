
namespace Core.Cameras.Extensoes;

public static class HttpResponseMessageExtensions
{
    public static bool ContemAssinatura(
        this HttpResponseMessage response,
        string body,
        string assinatura)
    {
        return body.Contains(assinatura, StringComparison.OrdinalIgnoreCase) ||
               response.Headers.Any(h =>
                   h.Value.Any(v =>
                       v.Contains(assinatura, StringComparison.OrdinalIgnoreCase)));
    }
}