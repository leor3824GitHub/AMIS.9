using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.User;

/// <summary>
/// Code-behind for PendingActions.razor
/// Displays pending issuances and asset acceptance workflows for end users
/// </summary>
public partial class PendingActions
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadPendingIssuances();
    }

    private async Task LoadPendingIssuances()
    {
        try
        {
            // Load pending issuances from API
            var searchCommand = new SearchIssuancesCommand
            {
                PageNumber = 1,
                PageSize = 100
            };
            
            var result = await ApiClient.SearchIssuancesEndpointAsync("1", searchCommand);
            
            if (result?.Items != null)
            {
                _issuances = result.Items
                    .Where(i => i.IsClosed == false) // Only open/pending issuances
                    .Select(i => new IssuanceDto
                    {
                        IssuanceId = i.Id ?? Guid.Empty,
                        AssetName = $"Asset #{(i.Id ?? Guid.Empty).ToString().Substring(0, 8)}", // TODO: Get actual asset name
                        IssuedDate = i.IssuanceDate,
                        Status = i.IsClosed ? "Closed" : "Pending"
                    })
                    .ToList();
                
                Snackbar?.Add($"Loaded {_issuances.Count} pending issuance(s)", Severity.Success);
            }
            else
            {
                _issuances = new List<IssuanceDto>();
                Snackbar?.Add("No pending issuances found", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading issuances: {ex.Message}", Severity.Error);
            _issuances = new List<IssuanceDto>();
        }
    }

    private void OpenAcceptDialog(IssuanceDto issuance)
    {
        _selectedIssuance = issuance;
        _employeeSignature = string.Empty;
        _remarks = string.Empty;
        _showAcceptDialog = true;
    }

    private void OpenRejectDialog(IssuanceDto issuance)
    {
        _selectedIssuance = issuance;
        _rejectionReason = string.Empty;
        _rejectionDetails = string.Empty;
        _showRejectDialog = true;
    }

    private async Task AcceptAsset()
    {
        if (_selectedIssuance == null) return;

        try
        {
            // Accept issuance via API
            var acceptCommand = new AcceptRequest
            {
                SignedByEmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000001") // TODO: Get from current user
            };
            
            var response = await ApiClient.AcceptIssuanceEndpointAsync("1", _selectedIssuance.IssuanceId, acceptCommand);
            
            if (response?.IssuanceId != Guid.Empty)
            {
                Snackbar?.Add("Asset accepted successfully", Severity.Success);
                _showAcceptDialog = false;
                
                // Reload to refresh the list
                await LoadPendingIssuances();
            }
            else
            {
                Snackbar?.Add("Failed to accept asset", Severity.Error);
            }
        }
        catch (ApiException apiEx)
        {
            Snackbar?.Add($"API Error: {apiEx.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error accepting asset: {ex.Message}", Severity.Error);
        }
    }

    private async Task RejectAsset()
    {
        if (_selectedIssuance == null) return;

        try
        {
            // Reject issuance via API
            var rejectCommand = new RejectRequest
            {
                RejectionReason = string.IsNullOrWhiteSpace(_rejectionDetails) 
                    ? _rejectionReason 
                    : $"{_rejectionReason}: {_rejectionDetails}"
            };
            
            var response = await ApiClient.RejectIssuanceEndpointAsync("1", _selectedIssuance.IssuanceId, rejectCommand);
            
            if (response?.IssuanceId != Guid.Empty)
            {
                Snackbar?.Add("Asset rejected successfully", Severity.Success);
                _showRejectDialog = false;
                
                // Reload to refresh the list
                await LoadPendingIssuances();
            }
            else
            {
                Snackbar?.Add("Failed to reject asset", Severity.Error);
            }
        }
        catch (ApiException apiEx)
        {
            Snackbar?.Add($"API Error: {apiEx.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error rejecting asset: {ex.Message}", Severity.Error);
        }
    }

    private Color GetStatusColor(string? status) => status?.ToLower() switch
    {
        "pending" => Color.Warning,
        "accepted" => Color.Success,
        "rejected" => Color.Error,
        _ => Color.Default
    };

    private sealed class IssuanceDto
    {
        public Guid IssuanceId { get; set; }
        public string AssetName { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? AcceptedDate { get; set; }
        public string? RejectionReason { get; set; }
    }
}
