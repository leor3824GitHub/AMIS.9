using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.Delete.v1;

public sealed class DeletePropertyCodeSequenceHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<DeletePropertyCodeSequenceCommand, DeletePropertyCodeSequenceResponse>
{
    private readonly IRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<DeletePropertyCodeSequenceResponse> Handle(
        DeletePropertyCodeSequenceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sequence = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        
        if (sequence is null)
            throw new KeyNotFoundException($"PropertyCodeSequence with ID {request.Id} not found.");

        await _repository.DeleteAsync(sequence, cancellationToken).ConfigureAwait(false);

        return new DeletePropertyCodeSequenceResponse(
            true,
            $"PropertyCodeSequence '{sequence.ClassCode}'-'{sequence.CategoryCode}' deleted successfully.");
    }
}
