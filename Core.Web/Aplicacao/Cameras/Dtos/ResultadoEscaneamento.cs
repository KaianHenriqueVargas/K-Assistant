using Core.Web.Models.Entidades;

namespace Core.Web.Aplicacao.Cameras.Dtos;

public class ResultadoEscaneamento
{
    public string NetworkRange { get; set; }
    public List<Camera> Cameras { get; set; } = new();
    public int TotalDispositivosEscaneados { get; set; }
}