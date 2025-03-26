using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Issuances.Update.v1;
public sealed class UpdateIssuanceHandler(
    ILogger<UpdateIssuanceHandler> logger,
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<UpdateIssuanceCommand, UpdateIssuanceResponse>
{
    public async Task<UpdateIssuanceResponse> Handle(UpdateIssuanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var issuance = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = issuance ?? throw new IssuanceNotFoundException(request.Id);
        var updatedIssuance = issuance.Update(request.EmployeeId, request.IssuanceDate, request.TotalAmount, request.Status);
        await repository.UpdateAsync(updatedIssuance, cancellationToken);
        logger.LogInformation("issuance with id : {IssuanceId} updated.", issuance.Id);
        return new UpdateIssuanceResponse(issuance.Id);
    }
}
