using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.List.v1;

public sealed class ListPropertyCodeSequencesHandler(
    [FromKeyedServices("inventories:propertyCodeSequences")] IReadRepository<PropertyCodeSequence> repository) 
    : IRequestHandler<ListPropertyCodeSequencesCommand, ListPropertyCodeSequencesResponse>
{
    private readonly IReadRepository<PropertyCodeSequence> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<ListPropertyCodeSequencesResponse> Handle(
        ListPropertyCodeSequencesCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new ListPropertyCodeSequencesSpec(request.PageNumber, request.PageSize, request.SearchTerm);
        var items = await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await _repository.CountAsync(
            new CountPropertyCodeSequencesSpec(request.SearchTerm), 
            cancellationToken).ConfigureAwait(false);

        var dtos = items.Select(x => new PropertyCodeSequenceDto(
            x.Id,
            x.ClassCode,
            x.Classification,
            x.CategoryCode,
            x.ItemCode,
            x.ItemDescription,
            x.GLAccount,
            x.LastSequenceValue)).ToList();

        return new ListPropertyCodeSequencesResponse(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }

    private sealed class ListPropertyCodeSequencesSpec : Specification<PropertyCodeSequence>
    {
        public ListPropertyCodeSequencesSpec(int pageNumber, int pageSize, string? searchTerm)
        {
            Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Query.Where(x => 
                    x.Classification.Contains(searchTerm) || 
                    x.CategoryCode.Contains(searchTerm) ||
                    x.ClassCode.Contains(searchTerm));
            }

            Query.OrderBy(x => x.ClassCode).ThenBy(x => x.CategoryCode);
        }
    }

    private sealed class CountPropertyCodeSequencesSpec : Specification<PropertyCodeSequence>
    {
        public CountPropertyCodeSequencesSpec(string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Query.Where(x => 
                    x.Classification.Contains(searchTerm) || 
                    x.CategoryCode.Contains(searchTerm) ||
                    x.ClassCode.Contains(searchTerm));
            }
        }
    }
}
