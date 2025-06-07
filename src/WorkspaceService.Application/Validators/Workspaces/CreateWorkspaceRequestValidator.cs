using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.Workspaces;

namespace WorkspaceService.Application.Validators.Workspaces;

public class CreateWorkspaceRequestValidator : AbstractValidator<CreateWorkspaceRequest>
{
    public CreateWorkspaceRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(4)
            .Matches(RegexConstants.EnglishLetters);
    }
}