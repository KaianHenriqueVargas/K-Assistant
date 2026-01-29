namespace Core.Cameras.Dtos;

public class ResultadoEscaneamento
{
    public string NetworkRange { get; set; }
    public List<Camera> Cameras { get; set; } = new();
    public int TotalDispositivosEscaneados { get; set; }
}