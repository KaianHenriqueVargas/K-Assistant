using Core.Cameras.Dtos;

namespace Core.Cameras.Abstracoes;

public interface IServicoDeEscaneamentoDeCameras
{
    /// <summary>
    /// Escaneia uma rede inteira em busca de câmeras.
    /// </summary>
    Task<ResultadoEscaneamento> ScanNetworkAsync(NetworkScanRequest request);

    /// <summary>
    /// Escaneia um único dispositivo (IP específico).
    /// </summary>
    Task<Camera?> ScanSingleDeviceAsync(string ipAddress);

    /// <summary>
    /// Retorna a lista de portas mais comuns utilizadas por câmeras.
    /// </summary>
    IReadOnlyCollection<int> GetCommonCameraPorts();
}