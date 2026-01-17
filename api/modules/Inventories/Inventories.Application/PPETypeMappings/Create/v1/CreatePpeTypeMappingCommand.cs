using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Services;
using FluentValidation;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.PPETypeMappings.Create.v1;

public record CreatePPETypeMappingCommand(
    string PPEType,
    string RCAAccountCode,
    string Description) : IRequest<Guid>;

public class CreatePPETypeMappingValidator : AbstractValidator<CreatePPETypeMappingCommand>
{
    public CreatePPETypeMappingValidator()
    {
        RuleFor(x => x.PPEType)
            .NotEmpty().WithMessage("PPE type is required.")
            .MaximumLength(100).WithMessage("PPE type must not exceed 100 characters.");

        RuleFor(x => x.RCAAccountCode)
            .NotEmpty().WithMessage("RCA account code is required.")
            .MaximumLength(50).WithMessage("RCA account code must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}

internal sealed class CreatePPETypeMappingHandler : IRequestHandler<CreatePPETypeMappingCommand, Guid>
{
    private readonly IPPETypeAccountMappingRepository _repository;

    public CreatePPETypeMappingHandler(IPPETypeAccountMappingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePPETypeMappingCommand request, CancellationToken cancellationToken)
    {
        // Check if PPE type already exists
        var existing = await _repository.GetByTypeAsync(request.PPEType, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"PPE type '{request.PPEType}' already exists.");
        }

        var mapping = PPETypeAccountMapping.Create(
            request.PPEType,
            request.RCAAccountCode,
            request.Description);

        await _repository.AddAsync(mapping, cancellationToken);

        return mapping.Id;
    }
}

