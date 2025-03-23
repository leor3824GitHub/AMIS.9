using FluentValidation;

namespace AMIS.Framework.Core.Identity.Roles.Features.CreateOrUpdateRole;

public class CreateOrUpdateRoleValidator : AbstractValidator<CreateOrUpdateRoleCommand>
{
    public CreateOrUpdateRoleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
    }
}
