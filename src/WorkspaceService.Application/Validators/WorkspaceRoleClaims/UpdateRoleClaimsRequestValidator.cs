using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;

namespace WorkspaceService.Application.Validators.WorkspaceRoleClaims;

public class UpdateRoleClaimsRequestValidator : AbstractValidator<UpdateRoleClaimsRequest>
{
    public UpdateRoleClaimsRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Value)
            .Matches(RegexConstants.EnglishLettersAndDot)
            .When(x => !string.IsNullOrWhiteSpace(x.Value));
    }
}