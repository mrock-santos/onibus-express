using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Application.Viagens;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("viagens")]
public class ViagensController : ControllerBase
{
    private readonly IMediator _mediator;

    public ViagensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /viagens?origem=São Paulo&destino=Rio de Janeiro&data=2026-08-01
    [HttpGet]
    public async Task<ActionResult<List<ViagemDto>>> Buscar(
        [FromQuery] string? origem,
        [FromQuery] string? destino,
        [FromQuery] DateOnly? data,
        CancellationToken cancellationToken)
    {
        var query = new BuscarViagensQuery(origem, destino, data);
        var viagens = await _mediator.Send(query, cancellationToken);
        return Ok(viagens);
    }

     // GET /viagens/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ViagemDetalhesDto>> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var viagem = await _mediator.Send(new ObterViagemPorIdQuery(id), cancellationToken);

        if (viagem is null)
            return NotFound(new { mensagem = "Viagem não encontrada." });

        return Ok(viagem);
    }
}