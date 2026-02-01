using Core.Web.Aplicacao.Cameras.Comandos;
using Core.Web.Aplicacao.Cameras.Dtos;
using Core.Web.Enumerados;
using Lib.Infraestrutura.Controllers;
using Lib.Infraestrutura.MediatR.Abstracoes;
using Lib.Infraestrutura.Validacoes;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Controllers;

[ApiExplorerSettings(GroupName = nameof(GruposDeEndpoints.Cameras))]
public class EscanearCamerasNaRedeController(
    IMediatorHandler bus,
    IValidadorDominio validadorDominio,
    IInicializador inicializador)
    : ControllerComandoPost<EscanearRedeComando, ResultadoEscaneamento>(bus, validadorDominio, inicializador);