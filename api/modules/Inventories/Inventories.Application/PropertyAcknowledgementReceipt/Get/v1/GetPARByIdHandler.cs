using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Get.v1;

public sealed class GetPARByIdHandler(
    ILogger<GetPARByIdHandler> logger,
    [FromKeyedServices("inventories:par")] IReadRepository<Domain.PropertyAcknowledgementReceipt> repository)
    : IRequestHandler<GetPARByIdQuery, GetPARByIdResponse>
{
    public async Task<GetPARByIdResponse> Handle(
        GetPARByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving PAR with ID: {Id}", request.Id);

        var par = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (par == null)
        {
            logger.LogWarning("PAR not found with ID: {Id}", request.Id);
            throw new KeyNotFoundException($"PAR with ID {request.Id} not found.");
        }

        var response = new GetPARByIdResponse
        {
            Id = par.Id,
            PARNumber = par.PARNumber,
            EmployeeId = par.EmployeeId,
            IssuanceDate = par.IssuanceDate,
            IssuancePurpose = par.IssuancePurpose,
            IssuanceLocation = par.IssuanceLocation,
            Status = par.Status.ToString(),
            LineItems = par.LineItems.Select(li => new PARLineItemDto
            {
                PropertyCode = li.PropertyCode,
                Description = li.Description,
                DateAcquired = li.DateAcquired,
                AcquisitionCost = li.AcquisitionCost,
                Condition = li.Condition,
                Remarks = li.Remarks
            }).ToList(),
            ReturnDate = par.ReturnDate,
            ReturnRemarks = par.ReturnRemarks,
            ReceivedByEmployeeId = par.ReceivedByEmployeeId,
            IssuedByName = par.IssuedByName,
            IssuedByDate = par.IssuedByDate,
            ReceivedByName = par.ReceivedByName,
            ReceivedByDate = par.ReceivedByDate,
            ApprovedByName = par.ApprovedByName,
            ApprovedByDate = par.ApprovedByDate,
            Notes = par.Notes,
            TotalAcquisitionCost = par.GetTotalAcquisitionCost(),
            CreatedAt = par.Created.DateTime,
            LastModifiedAt = par.LastModified.DateTime
        };

        logger.LogInformation("Successfully retrieved PAR {PARNumber}", par.PARNumber);
        return response;
    }
}
