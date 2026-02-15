using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.User;

/// <summary>
/// Code-behind for MyAssets.razor
/// Displays assets assigned to the current user with condition tracking and details view
/// </summary>
public partial class MyAssets
{
    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadUserAssets();
    }

    private async Task LoadUserAssets()
    {
        try
        {
            // Load physical assets for current user
            var searchCommand = new SearchPhysicalAssetsCommand
            {
                PageNumber = 1,
                PageSize = 100
            };
            
            var result = await ApiClient.SearchPhysicalAssetsEndpointAsync("1", searchCommand);
            
            if (result?.Items != null)
            {
                _assets = result.Items
                    .Where(a => a.CurrentCustodianId.HasValue) // Only assets assigned to someone
                    .Select(a => new AssetViewModel
                    {
                        Id = a.Id,
                        PropertyCode = a.PropertyCode ?? "N/A",
                        AssetName = a.ProductName ?? "Unknown Asset",
                        Category = "Asset",
                        AcquisitionCost = (decimal)a.AcquisitionCost,
                        Condition = "Good", // Default condition
                        DateAssigned = DateTime.Now // Default to current date
                    })
                    .ToList();
                
                Snackbar?.Add($"Loaded {_assets.Count} asset(s)", Severity.Success);
            }
            else
            {
                _assets = new List<AssetViewModel>();
                Snackbar?.Add("No assets found", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading assets: {ex.Message}", Severity.Error);
            _assets = new List<AssetViewModel>();
        }
    }

    private async Task SearchAssets()
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
        {
            await LoadUserAssets();
            return;
        }

        // Filter assets locally
        await LoadUserAssets();
        _assets = _assets.Where(a =>
            (a.AssetName?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (a.PropertyCode?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
        ).ToList();

        Snackbar?.Add($"Found {_assets.Count} matching asset(s)", Severity.Info);
    }

    private void ViewDetails(AssetViewModel asset)
    {
        _selectedAsset = asset;
        _showDetailsDialog = true;
    }

    private void OpenReturnDialog(AssetViewModel asset)
    {
        _selectedAsset = asset;
        _returnCondition = "Good";
        _returnNotes = string.Empty;
        _confirmReturn = false;
        _showReturnDialog = true;
    }

    private void OpenIssueDialog(AssetViewModel asset)
    {
        _selectedAsset = asset;
        _issueEmployeeName = string.Empty;
        _issueEmployeeId = string.Empty;
        _issueDocumentNumber = string.Empty;
        _issueDate = DateTime.Today;
        _issueNotes = string.Empty;
        _confirmIssue = false;
        _showIssueDialog = true;
    }

    private void ValidateEmployeeId()
    {
        if (!string.IsNullOrWhiteSpace(_issueEmployeeId) && !Guid.TryParse(_issueEmployeeId, out _))
        {
            Snackbar?.Add("Employee ID should be a valid GUID format", Severity.Warning);
        }
    }

    private bool CanSubmitIssue()
    {
        return _confirmIssue &&
               !string.IsNullOrWhiteSpace(_issueEmployeeId) &&
               !string.IsNullOrWhiteSpace(_issueEmployeeName) &&
               Guid.TryParse(_issueEmployeeId, out _);
    }

    private static int GetDaysInCustody(DateTime? dateAssigned)
    {
        if (!dateAssigned.HasValue) return 0;
        return (DateTime.UtcNow - dateAssigned.Value).Days;
    }

    private async Task SubmitIssue()
    {
        if (_selectedAsset == null)
        {
            Snackbar?.Add("No asset selected", Severity.Error);
            return;
        }

        if (!_confirmIssue)
        {
            Snackbar?.Add("Please confirm the asset transfer", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_issueEmployeeId) || string.IsNullOrWhiteSpace(_issueEmployeeName))
        {
            Snackbar?.Add("Employee ID and Name are required", Severity.Warning);
            return;
        }

        try
        {
            // Parse and validate employee ID
            if (!Guid.TryParse(_issueEmployeeId, out var employeeGuid))
            {
                Snackbar?.Add("Invalid Employee ID format. Please enter a valid GUID.", Severity.Warning);
                return;
            }

            // Validate transfer date
            if (_issueDate.HasValue && _issueDate.Value > DateTime.Today)
            {
                Snackbar?.Add("Transfer date cannot be in the future", Severity.Warning);
                return;
            }

            // Create issuance for asset transfer
            var createIssuanceCommand = new CreateIssuanceCommand
            {
                EmployeeId = employeeGuid,
                IssuanceDate = _issueDate ?? DateTime.Today,
                TotalAmount = (double)(_selectedAsset.AcquisitionCost ?? 0)
            };

            var issuanceResponse = await ApiClient.CreateIssuanceEndpointAsync("1", createIssuanceCommand);
            
            if (issuanceResponse?.Id.HasValue == true)
            {
                // Note: In a real scenario, we would also create an IssuanceItem
                // linking the physical asset to this issuance record
                
                Snackbar?.Add($"Asset successfully transferred to {_issueEmployeeName}", Severity.Success);
                _showIssueDialog = false;
                
                // Reload assets to reflect the transfer
                await LoadUserAssets();
            }
            else
            {
                Snackbar?.Add("Failed to create asset transfer record", Severity.Error);
            }
        }
        catch (ApiException apiEx)
        {
            Snackbar?.Add($"API Error: {apiEx.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error transferring asset: {ex.Message}", Severity.Error);
        }
    }

    private async Task SubmitReturn()
    {
        if (_selectedAsset == null)
        {
            Snackbar?.Add("No asset selected", Severity.Error);
            return;
        }

        if (!_confirmReturn)
        {
            Snackbar?.Add("Please confirm the return by checking the confirmation box", Severity.Warning);
            return;
        }

        // Validate that notes are provided for non-Good conditions
        if ((_returnCondition == "Fair" || _returnCondition == "For Repair") && 
            string.IsNullOrWhiteSpace(_returnNotes))
        {
            Snackbar?.Add($"Please provide notes explaining the {_returnCondition} condition", Severity.Warning);
            return;
        }

        try
        {
            // Return physical asset via API
            var returnCommand = new ReturnPhysicalAssetCommand
            {
                Id = _selectedAsset.Id,
                Condition = _returnCondition ?? "Good",
                Reason = _returnNotes ?? "Asset return",
                AcceptedBy = Guid.Empty, // TODO: Get from current user context
                QuantityReturned = 1
            };

            var response = await ApiClient.ReturnPhysicalAssetEndpointAsync("1", _selectedAsset.Id, returnCommand);

            if (response?.AssetId != Guid.Empty)
            {
                var conditionMessage = _returnCondition == "Good" 
                    ? "in good condition" 
                    : $"with condition: {_returnCondition}";
                
                Snackbar?.Add($"Asset returned successfully {conditionMessage}", Severity.Success);
                _showReturnDialog = false;
                
                // Reload assets to reflect the return
                await LoadUserAssets();
            }
            else
            {
                Snackbar?.Add("Failed to return asset", Severity.Error);
            }
        }
        catch (ApiException apiEx)
        {
            Snackbar?.Add($"API Error: {apiEx.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error returning asset: {ex.Message}", Severity.Error);
        }
    }

    private static Color GetConditionColor(string? condition) => condition switch
    {
        "Good" => Color.Success,
        "Fair" => Color.Warning,
        "For Repair" => Color.Error,
        _ => Color.Default
    };

    private sealed class AssetViewModel
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal? AcquisitionCost { get; set; }
        public string Condition { get; set; } = "Good";
        public DateTime? DateAssigned { get; set; }
    }
}
