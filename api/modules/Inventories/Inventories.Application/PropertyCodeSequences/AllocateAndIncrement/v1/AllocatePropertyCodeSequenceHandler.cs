using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.AllocateAndIncrement.v1;

/// <summary>
/// Handles allocation of property code sequences by classification and item description.
/// Returns the code components (ClassCode, CategoryCode, ItemCode) and next sequence number.
/// </summary>
public sealed class AllocatePropertyCodeSequenceHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<AllocatePropertyCodeSequenceCommand, AllocatePropertyCodeSequenceResponse>
{
    private readonly IRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<AllocatePropertyCodeSequenceResponse> Handle(
        AllocatePropertyCodeSequenceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.Classification))
            throw new ArgumentException("Classification is required.", nameof(request.Classification));
        if (string.IsNullOrWhiteSpace(request.ItemDescription))
            throw new ArgumentException("Item Description is required.", nameof(request.ItemDescription));

        // Fetch sequence tracker by classification and item description
        var spec = new PropertyCodeSequenceByClassificationAndItemDescriptionSpec(
            request.Classification, 
            request.ItemDescription);
        
        var sequence = await _repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException(
                $"No property code sequence found for classification '{request.Classification}' and item description '{request.ItemDescription}'. " +
                $"Please ensure the item has been properly configured in the system.");

        // Atomically increment and return the next sequence number with code components
        var nextSequence = sequence.Increment();
        await _repository.UpdateAsync(sequence, cancellationToken).ConfigureAwait(false);

        return new AllocatePropertyCodeSequenceResponse(
            sequence.ClassCode,
            sequence.CategoryCode,
            sequence.ItemCode,
            nextSequence);
    }

    /// <summary>
    /// Specification to find PropertyCodeSequence by classification and item description.
    /// </summary>
    private sealed class PropertyCodeSequenceByClassificationAndItemDescriptionSpec 
        : Specification<PropertyCodeSequence>
    {
        public PropertyCodeSequenceByClassificationAndItemDescriptionSpec(string classification, string itemDescription) =>
            Query.Where(x => x.Classification == classification && x.ItemDescription == itemDescription);
    }
}
