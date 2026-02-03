namespace AMIS.Blazor.Infrastructure.Api;

public partial interface IApiClient
{
    System.Threading.Tasks.Task<EmployeeResponsePagedList> SearchEmployeesAsync(
        string searchText,
        int pageNumber,
        int pageSize,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task<PARListResponse> GetPARListAsync(
        string? searchText,
        int? statusFilter,
        System.DateTime? startDate,
        System.DateTime? endDate,
        int pageNumber,
        int pageSize,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task<PARDetailsDto> GetPARByIdAsync(
        System.Guid id,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task<CreatePARResponse> CreatePARAsync(
        CreatePARCommand request,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task UpdatePARAsync(
        System.Guid id,
        UpdatePARCommand request,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task PostPARAsync(
        PostPARCommand request,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task CancelPARAsync(
        CancelPARCommand request,
        System.Threading.CancellationToken cancellationToken = default);

    System.Threading.Tasks.Task ReturnPARAsync(
        ReturnPARCommand request,
        System.Threading.CancellationToken cancellationToken = default);
}

public partial class ApiClient
{
    private const string ParApiVersion = "1";

    public System.Threading.Tasks.Task<EmployeeResponsePagedList> SearchEmployeesAsync(
        string searchText,
        int pageNumber,
        int pageSize,
        System.Threading.CancellationToken cancellationToken = default)
    {
        var request = new SearchEmployeesCommand
        {
            Keyword = searchText,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return SearchEmployeesEndpointAsync(ParApiVersion, request, cancellationToken);
    }

    public System.Threading.Tasks.Task<PARListResponse> GetPARListAsync(
        string? searchText,
        int? statusFilter,
        System.DateTime? startDate,
        System.DateTime? endDate,
        int pageNumber,
        int pageSize,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException<PARListResponse>(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task<PARDetailsDto> GetPARByIdAsync(
        System.Guid id,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException<PARDetailsDto>(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task<CreatePARResponse> CreatePARAsync(
        CreatePARCommand request,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException<CreatePARResponse>(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task UpdatePARAsync(
        System.Guid id,
        UpdatePARCommand request,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task PostPARAsync(
        PostPARCommand request,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task CancelPARAsync(
        CancelPARCommand request,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }

    public System.Threading.Tasks.Task ReturnPARAsync(
        ReturnPARCommand request,
        System.Threading.CancellationToken cancellationToken = default)
    {
        return System.Threading.Tasks.Task.FromException(
            new System.NotSupportedException("PAR endpoints are not available in the generated API client."));
    }
}

public sealed class PARListResponse
{
    public System.Collections.Generic.IReadOnlyList<PARListItemDto> Data { get; set; } =
        System.Array.Empty<PARListItemDto>();

    public int TotalCount { get; set; }
}

public sealed class PARListItemDto
{
    public System.Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public System.DateTime IssuanceDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ItemsCount { get; set; }
}

public enum PARStatus
{
    Draft = 0,
    Posted = 1,
    Returned = 2,
    Cancelled = 3
}

public sealed class PARLineItemDto
{
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public System.DateTimeOffset DateAcquired { get; set; }
    public decimal AcquisitionCost { get; set; }
    public string? Condition { get; set; }
    public string? Remarks { get; set; }
}

public sealed class PARDetailsDto
{
    public System.Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public System.Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Position { get; set; }
    public System.DateTimeOffset IssuanceDate { get; set; }
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public PARStatus Status { get; set; }
    public System.Collections.Generic.IReadOnlyList<PARLineItemDto> LineItems { get; set; } =
        System.Array.Empty<PARLineItemDto>();
    public System.DateTime? ReturnDate { get; set; }
    public string? ReturnNotes { get; set; }
    public string? IssuedByName { get; set; }
    public System.DateTimeOffset? IssuedByDate { get; set; }
    public string? ReceivedByName { get; set; }
    public System.DateTimeOffset? ReceivedByDate { get; set; }
    public string? ApprovedByName { get; set; }
    public System.DateTimeOffset? ApprovedByDate { get; set; }
    public string? Notes { get; set; }
}

public sealed class CreatePARLineItemRequest
{
    public string PropertyCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public System.DateTimeOffset DateAcquired { get; set; }
    public decimal AcquisitionCost { get; set; }
    public string? Condition { get; set; }
    public string? Remarks { get; set; }
}

public sealed class CreatePARCommand
{
    public string PARNumber { get; set; } = string.Empty;
    public System.Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public System.DateTimeOffset IssuanceDate { get; set; }
    public System.Collections.Generic.IReadOnlyList<CreatePARLineItemRequest> LineItems { get; set; } =
        System.Array.Empty<CreatePARLineItemRequest>();
    public string? Position { get; set; }
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public string? Notes { get; set; }
}

public sealed class UpdatePARCommand
{
    public System.Guid Id { get; set; }
    public System.Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public System.DateTimeOffset IssuanceDate { get; set; }
    public System.Collections.Generic.IReadOnlyList<CreatePARLineItemRequest> LineItems { get; set; } =
        System.Array.Empty<CreatePARLineItemRequest>();
    public string? Position { get; set; }
    public string? IssuancePurpose { get; set; }
    public string? IssuanceLocation { get; set; }
    public string? Notes { get; set; }
}

public sealed class CreatePARResponse
{
    public System.Guid Id { get; set; }
    public string PARNumber { get; set; } = string.Empty;
    public System.DateTime CreatedAt { get; set; }
}

public sealed class PostPARCommand
{
    public System.Guid Id { get; set; }
}

public sealed class CancelPARCommand
{
    public System.Guid Id { get; set; }
}

public sealed record ReturnPARCommand(
    System.Guid Id,
    System.DateTime ReturnDate,
    System.Guid ReceivedByEmployeeId,
    string ReceivedByEmployeeName,
    string? ReturnRemarks = null);

public sealed class EmployeeDto
{
    public System.Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Position { get; set; }
}
