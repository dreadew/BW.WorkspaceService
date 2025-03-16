using FluentValidation;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Application.Validators.WorkspaceRoleClaims;

public class CreateRoleClaimsRequestValidator : AbstractValidator<CreateRoleClaimsRequest>
{
    public CreateRoleClaimsRequestValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Value)
            .NotEmpty()
            .Matches(RegexConstants.EnglishLettersAndDot)
            .MinimumLength(3);
    }
}