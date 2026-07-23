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
            return CreatedAtAction(nameof(Criar), new { codigo = reserva.CodigoReserva }, reserva);
        }
        catch (ArgumentException ex)
        {
            // Lançada pelo construtor de Passageiro/Reserva quando algum dado é inválido
            // (ex: CPF inválido, e-mail inválido, número de assento <= 0)
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Lançada pelo Handler quando uma regra de negócio é violada
            // (ex: assento ocupado, viagem já realizada, viagem não encontrada)
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}