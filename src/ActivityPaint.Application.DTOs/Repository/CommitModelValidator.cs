using FluentValidation;

namespace ActivityPaint.Application.DTOs.Repository;

public class CommitModelValidator : AbstractValidator<CommitModel>
{
    public CommitModelValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty();
    }
}
