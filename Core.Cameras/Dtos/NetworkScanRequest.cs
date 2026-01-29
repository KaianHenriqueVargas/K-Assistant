namespace Core.Cameras.Dtos;

public class NetworkScanRequest
{
    public string? NetworkRange { get; set; }
    public int TimeoutMs { get; set; } = 2000;
    public int MaxThreads { get; set; } = 50;
    public bool DeepScan { get; set; } = false;
}