using FluentValidation;

namespace ActivityPaint.Application.DTOs.Repository;

public class AuthorModelValidator : AbstractValidator<AuthorModel>
{
    public AuthorModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FullName)
            .NotEmpty();
    }
}
