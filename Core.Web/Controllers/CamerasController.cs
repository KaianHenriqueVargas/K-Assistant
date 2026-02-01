using Core.Web.Aplicacao.Cameras.Comandos;
using Core.Web.Aplicacao.Cameras.Dtos;
using Lib.Infraestrutura.Controllers;
using Lib.Infraestrutura.MediatR.Abstracoes;
using Lib.Infraestrutura.Validacoes;

namespace Core.Web.Controllers;

public class EscanearCamerasNaRedeController(
    IMediatorHandler bus,
    IValidadorDominio validadorDominio,
    IInicializador inicializador)
    : ControllerComandoPost<EscanearRedeComando, ResultadoEscaneamento>(bus, validadorDominio, inicializador);