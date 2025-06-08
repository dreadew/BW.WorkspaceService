using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Application.Validators.WorkspaceRoles;

public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .Matches(RegexConstants.DefaultLettersAndNumbers);
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
    }
}