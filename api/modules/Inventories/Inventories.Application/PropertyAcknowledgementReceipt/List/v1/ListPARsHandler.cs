using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.List.v1;

public sealed class ListPARsHandler(
    ILogger<ListPARsHandler> logger,
    [FromKeyedServices("inventories:par")] IReadRepository<Domain.PropertyAcknowledgementReceipt> repository)
    : IRequestHandler<ListPARsQuery, ListPARsResponse>
{
    public async Task<ListPARsResponse> Handle(
        ListPARsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving list of Property Accountability Receipts");

        var pars = await repository.ListAsync(cancellationToken);

        var parDtos = pars
            .OrderByDescending(x => x.Created)
            .Select(x => new PARDto
            {
                Id = x.Id,
                PARNumber = x.PARNumber,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.EmployeeName,
                Department = x.Department,
                IssuanceDate = x.IssuanceDate,
                Status = x.Status.ToString(),
                LineItemsCount = x.GetLineItemsCount(),
                TotalAcquisitionCost = x.GetTotalAcquisitionCost(),
                ReturnDate = x.ReturnDate,
                CreatedAt = x.Created
            })
            .ToList();

        logger.LogInformation("Retrieved {Count} PARs", parDtos.Count);
        return new ListPARsResponse(parDtos);
    }
}
