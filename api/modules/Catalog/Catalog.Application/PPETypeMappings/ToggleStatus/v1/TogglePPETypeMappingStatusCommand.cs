using AMIS.WebApi.Catalog.Domain.Services;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PPETypeMappings.ToggleStatus.v1;

public record TogglePPETypeMappingStatusCommand(Guid Id, bool Activate) : IRequest<Unit>;

internal sealed class TogglePPETypeMappingStatusHandler : IRequestHandler<TogglePPETypeMappingStatusCommand, Unit>
{
    private readonly IPPETypeAccountMappingRepository _repository;

    public TogglePPETypeMappingStatusHandler(IPPETypeAccountMappingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(TogglePPETypeMappingStatusCommand request, CancellationToken cancellationToken)
    {
        var mapping = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (mapping == null)
        {
            throw new InvalidOperationException($"PPE type mapping with ID '{request.Id}' not found.");
        }

        if (request.Activate)
            mapping.Activate();
        else
            mapping.Deactivate();

        await _repository.UpdateAsync(mapping, cancellationToken);

        return Unit.Value;
    }
}
