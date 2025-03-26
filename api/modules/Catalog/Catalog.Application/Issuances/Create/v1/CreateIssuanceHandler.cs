using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Issuances.Create.v1;
public sealed class CreateIssuanceHandler(
    ILogger<CreateIssuanceHandler> logger,
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<CreateIssuanceCommand, CreateIssuanceResponse>
{
    public async Task<CreateIssuanceResponse> Handle(CreateIssuanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var issuance = Issuance.Create(request.EmployeeId, request.TotalAmount, request.Status);
        await repository.AddAsync(issuance, cancellationToken);
        logger.LogInformation("issuance created {IssuanceId}", issuance.Id);
        return new CreateIssuanceResponse(issuance.Id);
    }
}
