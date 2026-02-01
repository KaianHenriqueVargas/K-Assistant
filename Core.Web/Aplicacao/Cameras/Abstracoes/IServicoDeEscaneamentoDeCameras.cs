using Core.Web.Aplicacao.Cameras.Comandos;
using Core.Web.Aplicacao.Cameras.Dtos;

namespace Core.Web.Aplicacao.Cameras.Abstracoes;

public interface IServicoDeEscaneamentoDeCameras
{
    Task<ResultadoEscaneamento> ScanNetworkAsync(EscanearRedeComando request);
}