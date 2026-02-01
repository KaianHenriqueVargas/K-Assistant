using Core.Web.Aplicacao.Cameras.Abstracoes;
using Core.Web.Aplicacao.Cameras.Dtos;
using Core.Web.Infraestrutura.Network.Abstracoes;
using Core.Web.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Controllers;

[ApiController]
[Route("api/cameras")]

public sealed class CamerasController : ControllerBase
{
    private readonly IServicoDeEscaneamentoDeCameras _servicoDeEscaneamentoDeCameras;
    public CamerasController(IServicoDeEscaneamentoDeCameras servicoDeEscaneamentoDeCameras)
    {
        _servicoDeEscaneamentoDeCameras = servicoDeEscaneamentoDeCameras;
    }

    /// <summary>
    /// Escaneia uma faixa de rede em busca de câmeras
    /// </summary>
    [HttpPost("scan/network")]
    public async Task<ActionResult<ResultadoEscaneamento>> ScanNetwork(
        [FromBody] NetworkScanRequest request,
        [FromServices] IServicoDeDeteccaoDeRedeLocal servico)
    {
        if (string.IsNullOrWhiteSpace(request.NetworkRange))
            request.NetworkRange = servico.ObterNetworkRange();

        var resultado = await _servicoDeEscaneamentoDeCameras.ScanNetworkAsync(request);
        return Ok(resultado);
    }

}