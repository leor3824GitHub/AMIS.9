using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AMIS.Blazor.Infrastructure.Api;

#region Enums

/// <summary>
/// Budget type for procurement plans
/// </summary>
public enum BudgetType
{
    _0 = 0, // GAA
    _1 = 1, // TrustFund
    _2 = 2, // Corporate
    _3 = 3  // None
}

/// <summary>
/// Status for Procurement Plans
/// </summary>
public enum ProcurementPlanStatus
{
    _0 = 0, // None
    _1 = 1, // Draft
    _2 = 2, // PendingApproval
    _3 = 3, // Approved
    _4 = 4, // Rejected
    _5 = 5  // Cancelled
}

/// <summary>
/// Status for Annual Procurement Plans
/// </summary>
public enum AnnualProcurementPlanStatus
{
    _0 = 0, // None
    _1 = 1, // Draft
    _2 = 2, // PendingApproval
    _3 = 3, // Approved
    _4 = 4, // Rejected
    _5 = 5  // Cancelled
}

/// <summary>
/// Project type for procurement items
/// </summary>
public enum ProjectType
{
    _0 = 0, // None
    _1 = 1, // Goods
    _2 = 2, // Services
    _3 = 3  // Infrastructure
}

#endregion

#region Search Commands

/// <summary>
/// Search command for Annual Procurement Plans
/// </summary>
public class SearchAnnualProcurementPlansCommand
{
    [JsonPropertyName("keyword")]
    public string? Keyword { get; set; }

    [JsonPropertyName("fiscalYear")]
    public int? FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public AnnualProcurementPlanStatus? Status { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;

    [JsonPropertyName("orderBy")]
    public string[]? OrderBy { get; set; }
}

/// <summary>
/// Search command for Procurement Plans
/// </summary>
public class SearchProcurementPlansCommand
{
    [JsonPropertyName("keyword")]
    public string? Keyword { get; set; }

    [JsonPropertyName("fiscalYear")]
    public int? FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public ProcurementPlanStatus? Status { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid? DepartmentId { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;

    [JsonPropertyName("orderBy")]
    public string[]? OrderBy { get; set; }
}

#endregion

#region Procurement Plan DTOs

/// <summary>
/// Command to create a Procurement Plan
/// </summary>
public class CreateProcurementPlanCommand
{
    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("isSupplemental")]
    public bool IsSupplemental { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }
}

/// <summary>
/// Response for creating a Procurement Plan
/// </summary>
public class CreateProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

/// <summary>
/// Command to update a Procurement Plan
/// </summary>
public class UpdateProcurementPlanCommand
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("isSupplemental")]
    public bool IsSupplemental { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }
}

/// <summary>
/// Response for updating a Procurement Plan
/// </summary>
public class UpdateProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

/// <summary>
/// Paged list for Procurement Plan search results
/// </summary>
public class ProcurementPlanListItemResponsePagedList
{
    [JsonPropertyName("items")]
    public List<ProcurementPlanListItemResponse> Items { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// List item response for Procurement Plans
/// </summary>
public class ProcurementPlanListItemResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string? DepartmentName { get; set; }

    [JsonPropertyName("status")]
    public ProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("isSupplemental")]
    public bool IsSupplemental { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public decimal TotalBudget { get; set; }
}

/// <summary>
/// Response for getting a single Procurement Plan
/// </summary>
public class GetProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string? DepartmentName { get; set; }

    [JsonPropertyName("status")]
    public ProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("isSupplemental")]
    public bool IsSupplemental { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public decimal TotalBudget { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }

    [JsonPropertyName("submissionDate")]
    public DateTimeOffset? SubmissionDate { get; set; }

    [JsonPropertyName("approvedByUserId")]
    public Guid? ApprovedByUserId { get; set; }

    [JsonPropertyName("approvalDate")]
    public DateTimeOffset? ApprovalDate { get; set; }

    [JsonPropertyName("rejectionReason")]
    public string? RejectionReason { get; set; }

    [JsonPropertyName("items")]
    public List<ProcurementPlanItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Procurement Plan Item response
/// </summary>
public class ProcurementPlanItemResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("estimatedBudget")]
    public decimal EstimatedBudget { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Command to add an item to a Procurement Plan
/// </summary>
public class AddProcurementPlanItemCommand
{
    [JsonPropertyName("planId")]
    public Guid PlanId { get; set; }

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Response for adding an item to a Procurement Plan
/// </summary>
public class AddProcurementPlanItemResponse
{
    [JsonPropertyName("itemId")]
    public Guid ItemId { get; set; }
}

/// <summary>
/// Command to update an item in a Procurement Plan
/// </summary>
public class UpdateProcurementPlanItemCommand
{
    [JsonPropertyName("planId")]
    public Guid PlanId { get; set; }

    [JsonPropertyName("itemId")]
    public Guid ItemId { get; set; }

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Request body for approving a procurement plan
/// </summary>
public class ApproveProcurementPlanBody
{
    [JsonPropertyName("approvedByUserId")]
    public Guid ApprovedByUserId { get; set; }
}

/// <summary>
/// Request body for rejecting a procurement plan
/// </summary>
public class RejectProcurementPlanBody
{
    [JsonPropertyName("rejectedByUserId")]
    public Guid RejectedByUserId { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Search result for Procurement Plans - matches API PagedList response
/// </summary>
public class ProcurementPlanSearchResult
{
    [JsonPropertyName("items")]
    public List<ProcurementPlanListItem> Items { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// List item for Procurement Plan search results (used by extension methods)
/// </summary>
public class ProcurementPlanListItem
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public ProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public double TotalBudget { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string? DepartmentName { get; set; }

    [JsonPropertyName("isSupplemental")]
    public bool IsSupplemental { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }

    [JsonPropertyName("preparedDate")]
    public DateTimeOffset? PreparedDate { get; set; }

    [JsonPropertyName("submittedByUserId")]
    public Guid? SubmittedByUserId { get; set; }

    [JsonPropertyName("submittedDate")]
    public DateTimeOffset? SubmittedDate { get; set; }

    [JsonPropertyName("approvedByUserId")]
    public Guid? ApprovedByUserId { get; set; }

    [JsonPropertyName("approvedDate")]
    public DateTimeOffset? ApprovedDate { get; set; }
}

#endregion

#region Annual Procurement Plan DTOs

/// <summary>
/// Command to create an Annual Procurement Plan
/// </summary>
public class CreateAnnualProcurementPlanCommand
{
    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }
}

/// <summary>
/// Response for creating an Annual Procurement Plan
/// </summary>
public class CreateAnnualProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

/// <summary>
/// Command to update an Annual Procurement Plan
/// </summary>
public class UpdateAnnualProcurementPlanCommand
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }
}

/// <summary>
/// Response for updating an Annual Procurement Plan
/// </summary>
public class UpdateAnnualProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

/// <summary>
/// Paged list for Annual Procurement Plan search results
/// </summary>
public class AnnualProcurementPlanListItemResponsePagedList
{
    [JsonPropertyName("items")]
    public List<AnnualProcurementPlanListItemResponse> Items { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// List item response for Annual Procurement Plans
/// </summary>
public class AnnualProcurementPlanListItemResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public AnnualProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public decimal TotalBudget { get; set; }
}

/// <summary>
/// Response for getting a single Annual Procurement Plan
/// </summary>
public class GetAnnualProcurementPlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public AnnualProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public decimal TotalBudget { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }

    [JsonPropertyName("submissionDate")]
    public DateTimeOffset? SubmissionDate { get; set; }

    [JsonPropertyName("approvedByUserId")]
    public Guid? ApprovedByUserId { get; set; }

    [JsonPropertyName("approvalDate")]
    public DateTimeOffset? ApprovalDate { get; set; }

    [JsonPropertyName("rejectionReason")]
    public string? RejectionReason { get; set; }

    [JsonPropertyName("items")]
    public List<AnnualProcurementPlanItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Annual Procurement Plan Item response
/// </summary>
public class AnnualProcurementPlanItemResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("estimatedBudget")]
    public decimal EstimatedBudget { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Request body for approving an annual procurement plan
/// </summary>
public class ApproveAnnualProcurementPlanBody
{
    [JsonPropertyName("approvedByUserId")]
    public Guid ApprovedByUserId { get; set; }
}

/// <summary>
/// Request body for rejecting an annual procurement plan
/// </summary>
public class RejectAnnualProcurementPlanBody
{
    [JsonPropertyName("rejectedByUserId")]
    public Guid RejectedByUserId { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Command to add an item to an Annual Procurement Plan
/// </summary>
public class AddAnnualProcurementPlanItemCommand
{
    [JsonPropertyName("planId")]
    public Guid PlanId { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Response for adding an item to an Annual Procurement Plan
/// </summary>
public class AddAnnualProcurementPlanItemResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

/// <summary>
/// Command to update an item in an Annual Procurement Plan
/// </summary>
public class UpdateAnnualProcurementPlanItemCommand
{
    [JsonPropertyName("planId")]
    public Guid PlanId { get; set; }

    [JsonPropertyName("itemId")]
    public Guid ItemId { get; set; }

    [JsonPropertyName("departmentId")]
    public Guid DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("papCode")]
    public string? PapCode { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("projectType")]
    public ProjectType ProjectType { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("unitOfMeasure")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [JsonPropertyName("unitCost")]
    public decimal UnitCost { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = string.Empty;

    [JsonPropertyName("isEarlyProcurement")]
    public bool IsEarlyProcurement { get; set; }

    [JsonPropertyName("scheduleMonth")]
    public string ScheduleMonth { get; set; } = string.Empty;

    [JsonPropertyName("fundingSource")]
    public string FundingSource { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }
}

/// <summary>
/// Search result for Annual Procurement Plans - matches API PagedList response
/// </summary>
public class AnnualProcurementPlanSearchResult
{
    [JsonPropertyName("items")]
    public List<AnnualProcurementPlanListItem> Items { get; set; } = new();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// List item for Annual Procurement Plan search results (used by extension methods)
/// </summary>
public class AnnualProcurementPlanListItem
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("controlNumber")]
    public string ControlNumber { get; set; } = string.Empty;

    [JsonPropertyName("fiscalYear")]
    public int FiscalYear { get; set; }

    [JsonPropertyName("status")]
    public AnnualProcurementPlanStatus Status { get; set; }

    [JsonPropertyName("budgetType")]
    public BudgetType BudgetType { get; set; }

    [JsonPropertyName("totalBudget")]
    public double TotalBudget { get; set; }

    [JsonPropertyName("preparedByUserId")]
    public Guid PreparedByUserId { get; set; }

    [JsonPropertyName("preparedDate")]
    public DateTimeOffset? PreparedDate { get; set; }

    [JsonPropertyName("submittedByUserId")]
    public Guid? SubmittedByUserId { get; set; }

    [JsonPropertyName("submittedDate")]
    public DateTimeOffset? SubmittedDate { get; set; }

    [JsonPropertyName("approvedByUserId")]
    public Guid? ApprovedByUserId { get; set; }

    [JsonPropertyName("approvedDate")]
    public DateTimeOffset? ApprovedDate { get; set; }
}

/// <summary>
/// For UI compatibility - Approve command wrapper
/// </summary>
public class ApproveAnnualProcurementPlanCommand
{
    public Guid Id { get; set; }
    public Guid ApprovedByUserId { get; set; }
}

/// <summary>
/// For UI compatibility - Reject request wrapper
/// </summary>
public class RejectAnnualProcurementPlanRequest
{
    public Guid RejectedByUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

#endregion

#region Extension Methods

/// <summary>
/// Extension methods for the API client to provide workarounds for missing NSwag-generated methods.
/// These extensions will be deprecated once the API server is running and NSwag can regenerate the client properly.
/// </summary>
public static class ApiClientExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Search Annual Procurement Plans with proper response deserialization.
    /// Workaround for NSwag-generated method returning void.
    /// </summary>
    public static async Task<AnnualProcurementPlanSearchResult> SearchAnnualProcurementPlansAsync(
        this HttpClient httpClient,
        string version,
        SearchAnnualProcurementPlansCommand command,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/v{version}/catalog/annualProcurementPlans/search";
        var json = JsonSerializer.Serialize(command, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AnnualProcurementPlanSearchResult>(JsonOptions, cancellationToken);
        return result ?? new AnnualProcurementPlanSearchResult();
    }

    /// <summary>
    /// Search Procurement Plans with proper response deserialization.
    /// Workaround for NSwag-generated method returning void.
    /// </summary>
    public static async Task<ProcurementPlanSearchResult> SearchProcurementPlansAsync(
        this HttpClient httpClient,
        string version,
        SearchProcurementPlansCommand command,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/v{version}/catalog/procurementPlans/search";
        var json = JsonSerializer.Serialize(command, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<ProcurementPlanSearchResult>(JsonOptions, cancellationToken);
        return result ?? new ProcurementPlanSearchResult();
    }
}

#endregion
