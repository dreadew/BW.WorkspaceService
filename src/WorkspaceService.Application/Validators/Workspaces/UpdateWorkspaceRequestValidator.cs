using FluentValidation;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.Workspaces;

namespace WorkspaceService.Application.Validators.Workspaces;

public class UpdateWorkspaceRequestValidator : AbstractValidator<UpdateWorkspaceRequest>
{
    public UpdateWorkspaceRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .MinimumLength(4)
            .Matches(RegexConstants.EnglishLetters)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}