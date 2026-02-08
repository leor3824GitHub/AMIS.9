using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Create.v1;

public sealed class CreatePARHandler(
    ILogger<CreatePARHandler> logger,
    [FromKeyedServices("inventories:par")] IRepository<Domain.PropertyAcknowledgementReceipt> repository)
    : IRequestHandler<CreatePARCommand, CreatePARResponse>
{
    public async Task<CreatePARResponse> Handle(
        CreatePARCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Creating new PAR with number: {PARNumber} for employee: {EmployeeName}",
                request.PARNumber,
                request.EmployeeName);

            var par = new Domain.PropertyAcknowledgementReceipt(
                request.PARNumber,
                request.EmployeeId,
                request.EmployeeName,
                request.Department,
                request.IssuanceDate,
                request.Position,
                request.IssuancePurpose,
                request.IssuanceLocation,
                request.Notes);

            var lineItems = request.LineItems.Select(x => new Domain.PARLineItem(
                x.PropertyCode,
                x.Description,
                x.DateAcquired,
                x.AcquisitionCost,
                x.Condition,
                x.Remarks)).ToList();

            par.AddLineItems(lineItems);

            // Note: Signature information (IssuedByName, ReceivedByName, ApprovedByName) 
            // will be stored in the AssignmentHistory when assets are issued.
            // This consolidation approach tracks formal signatures in AssetAssignmentHistory
            // with DocumentType.PAR flag for formal document tracking.

            await repository.AddAsync(par, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PAR {PARNumber} created as Draft with {Count} line items.", 
                request.PARNumber, request.LineItems.Count);
            
            return new CreatePARResponse(
                par.Id,
                par.PARNumber,
                par.Created.DateTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating PAR {PARNumber}", request.PARNumber);
            throw;
        }
    }
}
