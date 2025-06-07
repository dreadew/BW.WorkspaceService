using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspacePositions;

namespace WorkspaceService.Application.Validators.WorkspacePositions;

public class UpdatePositionRequestValidator : AbstractValidator<UpdatePositionRequest>
{
    public UpdatePositionRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .Matches(RegexConstants.EnglishLettersAndSpace)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}