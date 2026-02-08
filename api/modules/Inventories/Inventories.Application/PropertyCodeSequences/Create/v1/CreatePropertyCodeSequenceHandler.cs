using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Create.v1;

public sealed class CreatePropertyCodeSequenceHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<CreatePropertyCodeSequenceCommand, CreatePropertyCodeSequenceResponse>
{
    private readonly IRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<CreatePropertyCodeSequenceResponse> Handle(
        CreatePropertyCodeSequenceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.Classification))
            throw new ArgumentException("Classification is required.", nameof(request.Classification));
        if (string.IsNullOrWhiteSpace(request.Category))
            throw new ArgumentException("Category is required.", nameof(request.Category));

        var sequence = PropertyCodeSequence.Create(request.Classification, request.Category);
        await _repository.AddAsync(sequence, cancellationToken).ConfigureAwait(false);

        return new CreatePropertyCodeSequenceResponse(
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
