using AMIS.WebApi.Catalog.Domain.Services;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PPETypeMappings.List.v1;

public record ListPPETypeMappingsQuery : IRequest<List<PPETypeMappingResponse>>;

public record PPETypeMappingResponse(
    Guid Id,
    string PPEType,
    string RCAAccountCode,
    string Description,
    bool IsActive);

internal sealed class ListPPETypeMappingsHandler : IRequestHandler<ListPPETypeMappingsQuery, List<PPETypeMappingResponse>>
{
    private readonly IPPETypeAccountMappingRepository _repository;

    public ListPPETypeMappingsHandler(IPPETypeAccountMappingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PPETypeMappingResponse>> Handle(ListPPETypeMappingsQuery request, CancellationToken cancellationToken)
    {
        var mappings = await _repository.GetAllAsync(cancellationToken);
        
        return mappings.Select(m => new PPETypeMappingResponse(
            m.Id,
            m.PPEType,
            m.RCAAccountCode,
            m.Description,
            m.IsActive
        )).ToList();
    }
}
