/*
using MediatR;

namespace OperatorInterface.BlazorApp.Application;

// Commands
public record AuthorizeOperatorCommand(string Login, string Password, WorkplaceCode WorkplaceCode) : IRequest<SessionId>;

public record StartWorkCommand(SessionId SessionId) : IRequest;

public record RequestClientCommand(SessionId SessionId) : IRequest;

public record StartClientSessionCommand(SessionId SessionId) : IRequest;

public record CompleteClientSessionCommand(SessionId SessionId) : IRequest;

public record MarkClientNotCameCommand(SessionId SessionId, string Reason) : IRequest;

public record CloseSessionCommand(SessionId SessionId) : IRequest;
*/