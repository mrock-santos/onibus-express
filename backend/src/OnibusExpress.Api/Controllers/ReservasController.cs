using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Application.Reservas;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("reservas")]
public class ReservasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /reservas
    [HttpPost]
    public async Task<ActionResult<ReservaDto>> Criar(
        [FromBody] CriarReservaDto dados,
        CancellationToken cancellationToken)
    {
        try
        {
            var reserva = await _mediator.Send(new CriarReservaCommand(dados), cancellationToken);
            return CreatedAtAction(nameof(ObterPorCodigo), new { codigo = reserva.CodigoReserva }, reserva);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    // GET /reservas/{codigo}
    [HttpGet("{codigo}")]
    public async Task<ActionResult<ReservaDetalhesDto>> ObterPorCodigo(
        string codigo,
        CancellationToken cancellationToken)
    {
        var reserva = await _mediator.Send(new ObterReservaPorCodigoQuery(codigo), cancellationToken);

        if (reserva is null)
            return NotFound(new { mensagem = "Reserva não encontrada." });

        return Ok(reserva);
    }

    // DELETE /reservas/{codigo}
    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Cancelar(string codigo, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new CancelarReservaCommand(codigo), cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}