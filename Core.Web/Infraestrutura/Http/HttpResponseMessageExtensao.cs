namespace Core.Web.Infraestrutura.Http;

public static class HttpResponseMessageExtensao
{
    public static bool ContemAssinatura(this HttpResponseMessage response, string body, string assinatura)
        => body.Contains(assinatura, StringComparison.OrdinalIgnoreCase) ||
           response.Headers.Any(h =>
               h.Value.Any(v =>
                   v.Contains(assinatura, StringComparison.OrdinalIgnoreCase)));
}