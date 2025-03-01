using FluentValidation;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;

namespace WorkspaceService.Application.Validators.WorkspaceDirectory;

public class CreateDirectoryRequestValidator : AbstractValidator<CreateDirectoryRequest>
{
    public CreateDirectoryRequestValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .NotEmpty()
            .Matches(RegexConstants.EnglishLettersAndSpace);
    }
}