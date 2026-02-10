using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.GetById.v1;

public sealed class GetPropertyCodeSequenceHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IReadRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<GetPropertyCodeSequenceCommand, GetPropertyCodeSequenceResponse>
{
    private readonly IReadRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<GetPropertyCodeSequenceResponse> Handle(
        GetPropertyCodeSequenceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sequence = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        
        if (sequence is null)
            throw new KeyNotFoundException($"PropertyCodeSequence with ID {request.Id} not found.");

        return new GetPropertyCodeSequenceResponse(
            sequence.Id,
            sequence.ClassCode,
            sequence.Classification,
            sequence.CategoryCode,
            sequence.ItemCode,
            sequence.ItemDescription,
            sequence.GLAccount,
            sequence.LastSequenceValue);
    }
}
