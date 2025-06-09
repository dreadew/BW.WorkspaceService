using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;

namespace WorkspaceService.Application.Validators.WorkspaceUsers;

public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}