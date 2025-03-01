using FluentValidation;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Application.Validators.WorkspaceRoles;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .Matches(RegexConstants.EnglishLettersAndNumbers)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}