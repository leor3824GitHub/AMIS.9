using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Get.v1;

public sealed class GetInspectionRequestHandler(
    [FromKeyedServices("inventories:inspectionRequests")] IReadRepository<InspectionRequest> repository,
    ICacheService cache)
    : IRequestHandler<GetInspectionRequestRequest, InspectionRequestResponse>
{
    public async Task<InspectionRequestResponse> Handle(GetInspectionRequestRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspectionRequest = await cache.GetOrSetAsync(
            $"inspectionRequest:{request.Id}",
            async () =>
            {
                var spec = new GetInspectionRequestSpecs(request.Id);
                var inspectionRequest = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inspectionRequest == null) throw new InspectionRequestNotFoundException(request.Id);
                return inspectionRequest;
            },
            cancellationToken: cancellationToken);

        return inspectionRequest!;
    }
}

