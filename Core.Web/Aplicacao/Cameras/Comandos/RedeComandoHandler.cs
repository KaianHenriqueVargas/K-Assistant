using Core.Web.Aplicacao.Cameras.Abstracoes;
using Core.Web.Aplicacao.Cameras.Dtos;
using Core.Web.Infraestrutura.Network.Abstracoes;
using Lib.Infraestrutura.MediatR;
using Lib.Infraestrutura.Validacoes;
using MediatR;

namespace Core.Web.Aplicacao.Cameras.Comandos;

public class RedeComandoHandler(
    IValidadorDominio validadorDominio,
    IServicoDeEscaneamentoDeCameras _servicoDeEscaneamentoDeCameras,
    IServicoDeDeteccaoDeRedeLocal servicoDeDeteccaoDeRedeLocal) : ComandoHandlerAutoValidador(validadorDominio),
    IRequestHandler<EscanearRedeComando, ResultadoEscaneamento>
{
    public async Task<ResultadoEscaneamento> Handle(EscanearRedeComando request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.NetworkRange))
            request.NetworkRange = servicoDeDeteccaoDeRedeLocal.ObterNetworkRange();

        return await _servicoDeEscaneamentoDeCameras.ScanNetworkAsync(request);
    }
}