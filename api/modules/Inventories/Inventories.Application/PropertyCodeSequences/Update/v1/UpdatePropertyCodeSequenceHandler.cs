using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Update.v1;

public sealed class UpdatePropertyCodeSequenceHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<UpdatePropertyCodeSequenceCommand, UpdatePropertyCodeSequenceResponse>
{
    private readonly IRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<UpdatePropertyCodeSequenceResponse> Handle(
        UpdatePropertyCodeSequenceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sequence = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        
        if (sequence is null)
            throw new KeyNotFoundException($"PropertyCodeSequence with ID {request.Id} not found.");

        // Update properties
        sequence.Classification = request.Classification;
        sequence.CategoryCode = request.Category;

        // Allow manual update of LastSequenceValue for administrative purposes
        if (request.LastSequenceValue != sequence.LastSequenceValue)
        {
            if (request.LastSequenceValue < 0)
                throw new ArgumentException("LastSequenceValue cannot be negative.", nameof(request.LastSequenceValue));
            
            sequence.LastSequenceValue = request.LastSequenceValue;
        }

        await _repository.UpdateAsync(sequence, cancellationToken).ConfigureAwait(false);

        return new UpdatePropertyCodeSequenceResponse(
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
