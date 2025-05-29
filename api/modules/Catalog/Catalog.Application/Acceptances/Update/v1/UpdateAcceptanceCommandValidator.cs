using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Update.v1;

public class UpdateAcceptanceCommandValidator : AbstractValidator<UpdateAcceptanceCommand>
{
    public UpdateAcceptanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.AcceptanceDate).NotEmpty();
        RuleFor(x => x.AcceptedBy).NotEmpty();
    }
}
