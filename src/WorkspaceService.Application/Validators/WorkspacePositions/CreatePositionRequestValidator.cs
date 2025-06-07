using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspacePositions;

namespace WorkspaceService.Application.Validators.WorkspacePositions;

public class CreatePositionRequestValidator : AbstractValidator<CreatePositionRequest>
{
    public CreatePositionRequestValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches(RegexConstants.EnglishLettersAndSpace);
    }
}