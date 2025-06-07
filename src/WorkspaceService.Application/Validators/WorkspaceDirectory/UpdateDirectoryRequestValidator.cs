using Common.Base.Constants;
using FluentValidation;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;

namespace WorkspaceService.Application.Validators.WorkspaceDirectory;

public class UpdateDirectoryRequestValidator : AbstractValidator<UpdateDirectoryRequest>
{
    public UpdateDirectoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(RegexConstants.Guid);
        RuleFor(x => x.Name)
            .Matches(RegexConstants.EnglishLettersAndSpace)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}