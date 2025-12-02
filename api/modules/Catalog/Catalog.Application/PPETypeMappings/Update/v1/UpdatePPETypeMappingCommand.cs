using AMIS.WebApi.Catalog.Domain.Services;
using FluentValidation;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PPETypeMappings.Update.v1;

public record UpdatePPETypeMappingCommand(
    Guid Id,
    string RCAAccountCode,
    string Description) : IRequest<Unit>;

public class UpdatePPETypeMappingValidator : AbstractValidator<UpdatePPETypeMappingCommand>
{
    public UpdatePPETypeMappingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required.");

        RuleFor(x => x.RCAAccountCode)
            .NotEmpty().WithMessage("RCA account code is required.")
            .MaximumLength(50).WithMessage("RCA account code must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}

internal sealed class UpdatePPETypeMappingHandler : IRequestHandler<UpdatePPETypeMappingCommand, Unit>
{
    private readonly IPPETypeAccountMappingRepository _repository;

    public UpdatePPETypeMappingHandler(IPPETypeAccountMappingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdatePPETypeMappingCommand request, CancellationToken cancellationToken)
    {
        var mapping = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (mapping == null)
        {
            throw new InvalidOperationException($"PPE type mapping with ID '{request.Id}' not found.");
        }

        mapping.Update(request.RCAAccountCode, request.Description);

        await _repository.UpdateAsync(mapping, cancellationToken);

        return Unit.Value;
    }
}
