namespace AMIS.Catalog.Application.Issuances.Features.Accept.v1;

public sealed record AcceptIssuanceResponse(
    Guid IssuanceId,
    DateTime AcceptedOn,
    Guid CustodianId);
