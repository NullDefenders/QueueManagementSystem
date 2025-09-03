using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperatorInterface.Core.Application.UseCases.Commands.AssignClient;
using OperatorInterface.Core.Application.UseCases.Commands.AuthorizeOperator;
using OperatorInterface.Core.Application.UseCases.Commands.CloseOperatorSession;
using OperatorInterface.Core.Application.UseCases.Commands.CompleteClientSession;
using OperatorInterface.Core.Application.UseCases.Commands.MarkClientAsNotCame;
using OperatorInterface.Core.Application.UseCases.Commands.OpenOperatorSession;
using OperatorInterface.Core.Application.UseCases.Commands.RequestClient;
using OperatorInterface.Core.Application.UseCases.Commands.StartClientSession;
using OperatorInterface.Core.Domain.Model.Exceptions;
using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Queries.UseCases.GetActiveOperatorSession;
using OperatorInterface.Queries.UseCases.GetOperatorSession;
using OperatorInterface.Queries.UseCases.Shared;


namespace OperatorInterface.Api.Adapters.Http;

[ApiController]
[Route("api/v1/operator/sessions")]
public class OperatorSessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public OperatorSessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("authorize")]
    public async Task<ActionResult<SessionId>> Authorize(AuthorizeOperatorCommand command)
    {
        try
        {
            var sessionId = await _mediator.Send(command);
            return Ok(sessionId);
        }
        catch (AuthorizationFailedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/open")]
    public async Task<ActionResult> OpenSession(Guid sessionId)
    {
        try
        {
            await _mediator.Send(new OpenOperatorSessionCommand(new SessionId(sessionId)));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/request-client")]
    public async Task<ActionResult> RequestClient(Guid sessionId)
    {
        try
        {
            await _mediator.Send(new RequestClientCommand(new SessionId(sessionId)));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/assign-client")]
    public async Task<ActionResult> AssignClient(Guid sessionId, AssignClientRequest request)
    {
        try
        {
            await _mediator.Send(new AssignClientCommand(new SessionId(sessionId), request.TicketNumber));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/start-client-session")]
    public async Task<ActionResult> StartClientSession(Guid sessionId)
    {
        try
        {
            await _mediator.Send(new StartClientSessionCommand(new SessionId(sessionId)));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/complete-client-session")]
    public async Task<ActionResult> CompleteClientSession(Guid sessionId)
    {
        try
        {
            await _mediator.Send(new CompleteClientSessionCommand(new SessionId(sessionId)));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/mark-client-not-came")]
    public async Task<ActionResult> MarkClientAsNotCame(Guid sessionId, MarkClientNotCameRequest request)
    {
        try
        {
            await _mediator.Send(new MarkClientAsNotCameCommand(new SessionId(sessionId), request.Reason));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/close")]
    public async Task<ActionResult> CloseSession(Guid sessionId)
    {
        try
        {
            await _mediator.Send(new CloseOperatorSessionCommand(new SessionId(sessionId)));
            return Ok();
        }
        catch (OperatorDomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{sessionId}")]
    public async Task<ActionResult<OperatorSessionDto>> GetSession(Guid sessionId)
    {
        try
        {
            var session = await _mediator.Send(new GetOperatorSessionQuery(new SessionId(sessionId)));
            return Ok(session);
        }
        catch (OperatorDomainException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("operator/{operatorId}/active")]
    public async Task<ActionResult<OperatorSessionDto>> GetActiveSession(string operatorId)
    {
        var session = await _mediator.Send(new GetActiveOperatorSessionQuery(new OperatorId(operatorId)));
        if (session == null)
            return NotFound($"No active session found for operator {operatorId}");

        return Ok(session);
    }
}

public record AssignClientRequest(string TicketNumber);
public record MarkClientNotCameRequest(string Reason);