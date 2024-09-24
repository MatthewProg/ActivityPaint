using FluentValidation;
using FluentValidation.Results;

namespace ActivityPaint.Application.BusinessLogic.Tests;

public static class ValidatorMockFactory
{
    public static Mock<IValidator<T>> CreateValid<T>(Times? callCount = null)
    {
        var mock = new Mock<IValidator<T>>();

        var setup = mock.Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                        .Returns(new ValidationResult());

        if (callCount is not null)
        {
            setup.Verifiable(callCount.Value);
        }

        return mock;
    }
}
