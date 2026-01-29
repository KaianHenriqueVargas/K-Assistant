using Core.Dominio.Cameras.Abstracoes;
using Core.Dominio.Cameras.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers;

[ApiController]
[Route("api/cameras")]
public sealed class CamerasController : ControllerBase
{
    private readonly IServicoDeEscaneamentoDeCameras _servico;

    public CamerasController(IServicoDeEscaneamentoDeCameras servico)
    {
        _servico = servico;
    }

    /// <summary>
    /// Escaneia uma faixa de rede em busca de câmeras
    /// </summary>
    [HttpPost("scan/network")]
    public async Task<ActionResult<ResultadoEscaneamento>> ScanNetwork(
        [FromBody] NetworkScanRequest request,
        [FromServices] IDetectorDeRedeLocal detector)
    {
        if (string.IsNullOrWhiteSpace(request.NetworkRange))
            request.NetworkRange = detector.ObterNetworkRange();

        var resultado = await _servico.ScanNetworkAsync(request);
        return Ok(resultado);
    }

    /// <summary>
    /// Escaneia um único IP
    /// </summary>
    [HttpGet("scan/device/{ip}")]
    [ProducesResponseType(typeof(Camera), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScanSingleDevice(string ip)
    {
        var camera = await _servico.ScanSingleDeviceAsync(ip);

        if (camera is null)
            return NotFound();

        return Ok(camera);
    }

    /// <summary>
    /// Retorna as portas mais comuns utilizadas por câmeras
    /// </summary>
    [HttpGet("ports/common")]
    [ProducesResponseType(typeof(IReadOnlyCollection<int>), StatusCodes.Status200OK)]
    public IActionResult GetCommonPorts()
    {
        var portas = _servico.GetCommonCameraPorts();
        return Ok(portas);
    }
}