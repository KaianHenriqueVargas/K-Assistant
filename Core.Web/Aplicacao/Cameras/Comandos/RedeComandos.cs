using Core.Web.Aplicacao.Cameras.Dtos;
using Lib.Infraestrutura.MediatR.Abstracoes;

namespace Core.Web.Aplicacao.Cameras.Comandos;

public class EscanearRedeCommand : Comando<ResultadoEscaneamento>
{
    public string? NetworkRange { get; set; }
    public int TimeoutMs { get; set; } = 2000;
    public int MaxThreads { get; set; } = 50;
    public bool DeepScan { get; set; } = false;
}