namespace AMIS.Blazor.Infrastructure.Api;

/// <summary>
/// Minimal employee DTO for UI use only.
/// </summary>
public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}

/// <summary>
/// Extension methods for IApiClient to provide convenience methods mapping to auto-generated endpoint methods.
/// This file serves as a minimal bridge layer between UI code and the NSwag-generated ApiClient.
/// </summary>
public static class ApiClientExtensions
{
    // List/Search operations
    public static Task<ListPARsResponse> GetPARListAsync(
        this IApiClient client) =>
        client.ListPARsEndpointAsync("1.0");

    // Get by ID operations  
    public static Task<GetPARByIdResponse> GetPARByIdAsync(
        this IApiClient client, Guid id) =>
        client.GetPAREndpointAsync("1.0", id);

    // Create operations
    public static Task<CreatePARResponse> CreatePARAsync(
        this IApiClient client, CreatePARCommand command) =>
        client.CreatePAREndpointAsync("1.0", command);

    // Update operations
    public static Task<UpdatePARResponse> UpdatePARAsync(
        this IApiClient client, Guid id, UpdatePARCommand command) =>
        client.UpdatePAREndpointAsync("1.0", id, command);

    // Post operations (acknowledge/receipt)
    public static Task<PostPARResponse> PostPARAsync(
        this IApiClient client, Guid id) =>
        client.PostPAREndpointAsync("1.0", id);

    // Cancel operations
    public static Task<CancelPARResponse> CancelPARAsync(
        this IApiClient client, Guid id) =>
        client.CancelPAREndpointAsync("1.0", id);

    // Return operations
    public static Task<ReturnPARResponse> ReturnPARAsync(
        this IApiClient client, ReturnPARCommand command) =>
        client.ReturnPAREndpointAsync("1.0", command.Id, command);

    // Employee search - maps to the generated list endpoint if available
    public static async Task<ICollection<EmployeeResponse>> SearchEmployeesAsync(
        this IApiClient client, string? searchTerm = null)
    {
        try
        {
            // TODO: When Employee endpoints are available, use those instead
            // For now returning empty collection
            return new List<EmployeeResponse>();
        }
        catch
        {
            return new List<EmployeeResponse>();
        }
    }
}
