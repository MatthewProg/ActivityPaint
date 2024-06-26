using ActivityPaint.Core.Shared.Errors;

namespace ActivityPaint.Core.Shared.Result;

public class Result<T> : Result
{
    private readonly T? _value;

    private Result(bool isSuccess, T? value, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public T? Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("There is no value for failed result");
            }

            return _value;
        }
    }

    public static Result<T> Success(T? value) => new(true, value, Error.None);

    public static implicit operator Result<T>(T? value) => Success(value);
}
