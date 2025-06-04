using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public sealed record CreateInspectionRequestCommand(
    [property: DefaultValue("beef1122-xxxx-xxxx-xxxx-aabbccddeeff")] Guid PurchaseId,
    [property: DefaultValue("bfb91a20-xxxx-xxxx-xxxx-df0c914c1a22")] Guid RequestById
) : IRequest<CreateInspectionRequestResponse>;


