using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Application.Rotas;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("rotas")]
public class RotasController : ControllerBase
{
    private readonly IMediator _mediator;

    public RotasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /rotas
    [HttpGet]
    public async Task<ActionResult<List<RotaDto>>> Listar(CancellationToken cancellationToken)
    {
        var rotas = await _mediator.Send(new ListarRotasQuery(), cancellationToken);
        return Ok(rotas);
    }
}