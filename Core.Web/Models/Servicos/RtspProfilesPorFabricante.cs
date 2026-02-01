using Core.Web.Models.Dtos;
using Core.Web.Models.Enumerados;

namespace Core.Web.Models.Servicos;

public static class RtspProfilesPorFabricante
{
    public static IReadOnlyCollection<RtspProfile> Obter(FabricanteCamera fabricante)
    {
        return fabricante switch
        {
            FabricanteCamera.Hikvision =>
            [
                new RtspProfile("Streaming/Channels/101"),
                new RtspProfile("onvif1")
            ],

            FabricanteCamera.Dahua =>
            [
                new RtspProfile("cam/realmonitor?channel=1&subtype=0"),
                new RtspProfile("onvif1")
            ],

            _ =>
            [
                new RtspProfile("onvif1"),
                new RtspProfile("Streaming/Channels/101")
            ]
        };
    }
}
