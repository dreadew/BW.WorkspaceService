using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Application.Validators.WorkspaceUsers;

public class InviteUserRequestValidator : AbstractValidator<InviteUserRequest>
{
    public InviteUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.UserId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
    }
}