using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed record UpdatePurchaseCommand(
    Guid Id,           // The ID of the purchase to update
    Guid? SupplierId,          // The optional supplier ID (could be null)
    DateTime? PurchaseDate,    // The optional purchase date
    decimal TotalAmount,       // The total amount (could be recalculated)
    PurchaseStatus? Status,           // The optional purchase status
    string? ReferenceNumber,
    string? Notes,
    string? Currency
) : IRequest<UpdatePurchaseResponse>;
