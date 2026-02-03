using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.AssetIssuance.Components;

public partial class AssetIssuanceDialog
{
    [Parameter]
    public EventCallback OnCreateSuccess { get; set; }

    [Parameter]
    public EventCallback OnEditSuccess { get; set; }

    [Inject]
    private IApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private MudStepper? _stepper;
    private MudDataGrid<InventoryItemForIssuance>? _inventoryGrid;
    private bool _visible;

    private bool _isEditMode;
    private Guid? _editingIssuanceId;
    private bool _step1Completed;
    private bool _step2Completed;
    private int _currentStep;

    private EmployeeResponse? _selectedCustodian;
    private List<InventoryItemForIssuance> _selectedItems = new();
    private string _documentType = "PAR"; // Will be determined based on asset cost
    private decimal _totalIssuanceAmount;

    private List<EmployeeResponse> _employeeCache = new();
    private List<InventoryItemForIssuance> _inventoryCache = new();

    public async Task OpenCreateDialog()
    {
        ResetDialog();
        _isEditMode = false;
        _visible = true;
        StateHasChanged();
        await Task.CompletedTask;
    }

    public async Task OpenEditDialog(Guid issuanceId)
    {
        ResetDialog();
        _isEditMode = true;
        _editingIssuanceId = issuanceId;
        _visible = true;
        StateHasChanged();
        await LoadIssuanceForEditing(issuanceId);
    }

    private void ResetDialog()
    {
        _selectedCustodian = null;
        _selectedItems.Clear();
        _totalIssuanceAmount = 0;
        _step1Completed = false;
        _step2Completed = false;
        _currentStep = 0;
        _documentType = "PAR";
    }

    private async Task<IEnumerable<EmployeeResponse>> SearchEmployees(string searchValue, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchValue))
            return Enumerable.Empty<EmployeeResponse>();

        try
        {
            var result = await ApiClient.SearchEmployeesEndpointAsync("1", new SearchEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 10,
                Keyword = searchValue
            }, cancellationToken);

            return result?.Items ?? Enumerable.Empty<EmployeeResponse>();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error searching employees: {ex.Message}", Severity.Error);
            return Enumerable.Empty<EmployeeResponse>();
        }
    }

    private async Task<GridData<InventoryItemForIssuance>> LoadAvailableInventory(GridState<InventoryItemForIssuance> state)
    {
        try
        {
            var response = await ApiClient.SearchInventoriesEndpointAsync("1", new SearchInventoriesCommand
            {
                PageNumber = state.Page + 1,
                PageSize = state.PageSize == 0 ? 10 : state.PageSize
            });

            var inventoryItems = response?.Items?.Select(inv => new InventoryItemForIssuance
            {
                ProductId = inv.ProductId,
                ProductName = inv.Product?.Name ?? "Unknown",
                Category = inv.Product?.Category?.Name ?? "N/A",
                PropertyCode = inv.Product?.Sku.ToString() ?? "N/A",
                Classification = DetermineClassification((decimal)inv.AvePrice),
                AvailableQty = inv.Qty,
                UnitPrice = (decimal)inv.AvePrice,
                QuantityToIssue = 1
            }).ToList() ?? new List<InventoryItemForIssuance>();

            return new GridData<InventoryItemForIssuance>
            {
                Items = inventoryItems,
                TotalItems = response?.TotalCount ?? inventoryItems.Count
            };
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading inventory: {ex.Message}", Severity.Error);
            return new GridData<InventoryItemForIssuance> { Items = Enumerable.Empty<InventoryItemForIssuance>(), TotalItems = 0 };
        }
    }

    private string DetermineClassification(decimal cost)
    {
        return cost > 50000 ? "PPE" : "Semi-Expendable";
    }

    private Color GetClassificationColor(string classification) =>
        classification == "PPE" ? Color.Warning : Color.Info;

    private void RemoveSelectedItem(InventoryItemForIssuance item)
    {
        _selectedItems.Remove(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        _totalIssuanceAmount = _selectedItems.Sum(x => x.UnitPrice * x.QuantityToIssue);
        DetermineDocumentType();
    }

    private void DetermineDocumentType()
    {
        // If any selected item is PPE (> 50k), document is PAR, otherwise ICS
        _documentType = _selectedItems.Any(x => x.UnitPrice > 50000) ? "PAR" : "ICS";
    }

    private Task AdvanceToStep2()
    {
        _step1Completed = true;
        _currentStep = 1;
        return Task.CompletedTask;
    }

    private Task AdvanceToStep3()
    {
        _step2Completed = true;
        _currentStep = 2;
        return Task.CompletedTask;
    }

    private Task GoBackToStep1()
    {
        _currentStep = 0;
        return Task.CompletedTask;
    }

    private Task GoBackToStep2()
    {
        _currentStep = 1;
        return Task.CompletedTask;
    }

    private void CloseCustodianStep()
    {
        _visible = false;
        StateHasChanged();
    }

    private async Task ConfirmIssuance()
    {
        if (_selectedCustodian == null || !_selectedItems.Any())
        {
            Snackbar?.Add("Please select custodian and at least one item", Severity.Warning);
            return;
        }

        try
        {
            var command = new CreateIssuanceCommand
            {
                EmployeeId = _selectedCustodian.Id ?? Guid.Empty,
                IssuanceDate = DateTime.Now,
                TotalAmount = (double)_totalIssuanceAmount
            };

            await ApiClient.CreateIssuanceEndpointAsync("1", command);

            Snackbar?.Add($"Asset issuance created successfully! ({_documentType} generated)", Severity.Success);
            _visible = false;
            StateHasChanged();
            await OnCreateSuccess.InvokeAsync();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error creating issuance: {ex.Message}", Severity.Error);
        }
    }

    private async Task LoadIssuanceForEditing(Guid issuanceId)
    {
        try
        {
            var issuance = await ApiClient.GetIssuanceEndpointAsync("1", issuanceId);
            if (issuance != null)
            {
                // Load custodian and items for editing
                // This would populate the dialog with existing data
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading issuance: {ex.Message}", Severity.Error);
        }
    }
}

public class InventoryItemForIssuance
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string Category { get; set; } = "";
    public string PropertyCode { get; set; } = "";
    public string Classification { get; set; } = "";
    public int AvailableQty { get; set; }
    public decimal UnitPrice { get; set; }
    public int QuantityToIssue { get; set; }
}

public class IssuanceItemDto
{
    public Guid ProductId { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Status { get; set; }
}

