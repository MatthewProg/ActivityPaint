using ActivityPaint.Core.Shared.Result;
using Mediator;

namespace ActivityPaint.Application.Shared;

internal interface IResultCommand : ICommand<Result> { }
internal interface IResultCommand<TResponse> : ICommand<Result<TResponse>> { }
