namespace Core.Web.Models.Entidades;

public class Camera
{
    public string EnderecoDeIP { get; set; }
    public int Porta { get; set; }
    public string Fabricante { get; set; }
    public string Protocolo { get; set; }
    public bool EstaOnline { get; set; }
    
}