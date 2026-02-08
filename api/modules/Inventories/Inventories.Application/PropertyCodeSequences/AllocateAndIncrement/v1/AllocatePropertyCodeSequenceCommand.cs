using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyCodeSequences.AllocateAndIncrement.v1;

/// <summary>
/// Allocates a property code by classification and item description.
/// Returns the ClassCode, CategoryCode, and ItemCode components needed for property code generation.
/// </summary>
public sealed record AllocatePropertyCodeSequenceCommand(
    string Classification,
    string ItemDescription) : IRequest<AllocatePropertyCodeSequenceResponse>;

/// <summary>
/// Response containing the code components for property code generation.
/// </summary>
public sealed record AllocatePropertyCodeSequenceResponse(
    string ClassCode,
    string CategoryCode,
    string ItemCode,
    int NextSequenceNumber);
