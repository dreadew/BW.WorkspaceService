using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;

namespace WorkspaceService.Application.Validators.WorkspaceDirectory;

public class CreateDirectoryRequestValidator : AbstractValidator<CreateDirectoryRequest>
{
    public CreateDirectoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches(RegexConstants.DefaultLettersAndNumbers);
    }
}