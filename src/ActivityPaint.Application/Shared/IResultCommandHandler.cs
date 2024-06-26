using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.Shared;

internal interface IResultCommandHandler<in TCommand> : ICommandHandler<TCommand, Result>
     where TCommand : IResultCommand { }

internal interface IResultCommandHandler<in TCommand, TResponse> : ICommandHandler<TCommand, Result<TResponse>>
    where TCommand : IResultCommand<TResponse> { }
