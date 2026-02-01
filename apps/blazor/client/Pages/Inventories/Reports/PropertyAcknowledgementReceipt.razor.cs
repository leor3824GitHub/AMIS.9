using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PropertyAcknowledgementReceipt
{
    [Inject] private IApiClient ApiClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private Guid? _reportId;
    private PARModel _model = new();
    private EmployeeDto? _selectedEmployee;
    private int _reportStatus = 0; // 0=Draft, 1=Posted, 2=Returned, 3=Cancelled
    private string _reportStatusText = "Draft";
    private bool _isReadOnly;
    private string _actionButtonText = "Save Draft";
    private bool _showLineItemDialog;
    private PARLineItemModel _currentLineItem = new();
    private PARLineItemModel? _editingLineItem;
    private DialogOptions _dialogOptions = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };

    private bool CanSave => !string.IsNullOrWhiteSpace(_model.PARNumber) 
        && !string.IsNullOrWhiteSpace(_model.EmployeeName)
        && !string.IsNullOrWhiteSpace(_model.Department)
        && _model.IssuanceDate.HasValue;

    private bool CanPost => CanSave && _model.LineItems.Count > 0;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(ReportId) && Guid.TryParse(ReportId, out var id))
        {
            _reportId = id;
            await LoadReportAsync(id);
        }
        else
        {
            _model.PARNumber = await GeneratePARNumberAsync();
            _model.IssuanceDate = DateTime.Today;
        }
    }

    private async Task LoadReportAsync(Guid id)
    {
        try
        {
            var response = await ApiClient.GetPARByIdAsync(id);
            _reportId = response.Id;
            _model.PARNumber = response.PARNumber;
            _model.EmployeeId = response.EmployeeId;
            _model.EmployeeName = response.EmployeeName;
            _model.Department = response.Department;
            _model.Position = response.Position;
            _model.IssuanceDate = response.IssuanceDate.DateTime;
            _model.IssuancePurpose = response.IssuancePurpose;
            _model.IssuanceLocation = response.IssuanceLocation;
            _model.Notes = response.Notes;
            _model.IssuedByName = response.IssuedByName;
            _model.IssuedByDate = response.IssuedByDate?.DateTime;
            _model.ReceivedByName = response.ReceivedByName;
            _model.ReceivedByDate = response.ReceivedByDate?.DateTime;
            _model.ApprovedByName = response.ApprovedByName;
            _model.ApprovedByDate = response.ApprovedByDate?.DateTime;

            _model.LineItems = response.LineItems?.Select(x => new PARLineItemModel
            {
                PropertyCode = x.PropertyCode,
                Description = x.Description,
                DateAcquired = x.DateAcquired.DateTime,
                AcquisitionCost = x.AcquisitionCost,
                Condition = x.Condition,
                Remarks = x.Remarks
            }).ToList() ?? new();

            _reportStatus = (int)response.Status;
            _reportStatusText = response.Status.ToString();
            _actionButtonText = _reportStatus == 0 ? "Update" : "View";
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading PAR: {ex.Message}", Severity.Error);
        }
    }

    private async Task<string> GeneratePARNumberAsync()
    {
        return $"PAR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private async Task<IEnumerable<EmployeeDto>> SearchEmployees(string searchText, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return Array.Empty<EmployeeDto>();

        try
        {
            var response = await ApiClient.SearchEmployeesAsync(searchText, 1, 20, cancellationToken);
            return response.Data ?? Array.Empty<EmployeeDto>();
        }
        catch
        {
            return Array.Empty<EmployeeDto>();
        }
    }

    private void OnEmployeeSelected(EmployeeDto? employee)
    {
        if (employee != null)
        {
            _model.EmployeeId = employee.Id;
            _model.EmployeeName = employee.Name;
            _model.Department = employee.Department ?? "";
            _model.Position = employee.Position;
        }
    }

    private void AddLineItem()
    {
        _currentLineItem = new PARLineItemModel
        {
            DateAcquired = DateTime.Today,
            AcquisitionCost = 0
        };
        _editingLineItem = null;
        _showLineItemDialog = true;
    }

    private void EditLineItem(PARLineItemModel item)
    {
        _editingLineItem = item;
        _currentLineItem = new PARLineItemModel
        {
            PropertyCode = item.PropertyCode,
            Description = item.Description,
            DateAcquired = item.DateAcquired,
            AcquisitionCost = item.AcquisitionCost,
            Condition = item.Condition,
            Remarks = item.Remarks
        };
        _showLineItemDialog = true;
    }

    private void SaveLineItem()
    {
        if (_editingLineItem != null)
        {
            _editingLineItem.PropertyCode = _currentLineItem.PropertyCode;
            _editingLineItem.Description = _currentLineItem.Description;
            _editingLineItem.DateAcquired = _currentLineItem.DateAcquired;
            _editingLineItem.AcquisitionCost = _currentLineItem.AcquisitionCost;
            _editingLineItem.Condition = _currentLineItem.Condition;
            _editingLineItem.Remarks = _currentLineItem.Remarks;
        }
        else
        {
            _model.LineItems.Add(_currentLineItem);
        }
        _showLineItemDialog = false;
    }

    private void CancelLineItemEdit()
    {
        _showLineItemDialog = false;
    }

    private void RemoveLineItem(PARLineItemModel item)
    {
        _model.LineItems.Remove(item);
    }

    private async Task SubmitAsync()
    {
        try
        {
            var lineItems = _model.LineItems.Select(x => new CreatePARLineItemRequest
            {
                PropertyCode = x.PropertyCode,
                Description = x.Description,
                DateAcquired = new DateTimeOffset(x.DateAcquired),
                AcquisitionCost = x.AcquisitionCost,
                Condition = x.Condition,
                Remarks = x.Remarks
            }).ToList();

            if (_reportId.HasValue)
            {
                var updateRequest = new UpdatePARCommand
                {
                    Id = _reportId.Value,
                    EmployeeId = _model.EmployeeId,
                    EmployeeName = _model.EmployeeName,
                    Department = _model.Department,
                    IssuanceDate = new DateTimeOffset(_model.IssuanceDate ?? DateTime.Today),
                    Position = _model.Position,
                    IssuancePurpose = _model.IssuancePurpose,
                    IssuanceLocation = _model.IssuanceLocation,
                    Notes = _model.Notes,
                    LineItems = lineItems
                };

                await ApiClient.UpdatePARAsync(_reportId.Value, updateRequest);
                Snackbar.Add("PAR updated successfully", Severity.Success);
            }
            else
            {
                var createRequest = new CreatePARCommand
                {
                    PARNumber = _model.PARNumber,
                    EmployeeId = _model.EmployeeId,
                    EmployeeName = _model.EmployeeName,
                    Department = _model.Department,
                    IssuanceDate = new DateTimeOffset(_model.IssuanceDate ?? DateTime.Today),
                    Position = _model.Position,
                    IssuancePurpose = _model.IssuancePurpose,
                    IssuanceLocation = _model.IssuanceLocation,
                    Notes = _model.Notes,
                    LineItems = lineItems
                };

                var response = await ApiClient.CreatePARAsync(createRequest);
                _reportId = response.Id;
                Snackbar.Add("PAR created successfully", Severity.Success);
                Navigation.NavigateTo($"/inventories/reports/par/{_reportId}");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error saving PAR: {ex.Message}", Severity.Error);
        }
    }

    private async Task PostAsync()
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Post",
            "Posting this PAR will assign the PPE items to the custodian and lock the document. Continue?",
            yesText: "Post", cancelText: "Cancel");

        if (confirmed == true && _reportId.HasValue)
        {
            try
            {
                await ApiClient.PostPARAsync(new PostPARCommand { Id = _reportId.Value });
                Snackbar.Add("PAR posted and items assigned successfully", Severity.Success);
                await LoadReportAsync(_reportId.Value);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error posting PAR: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ReturnAsync()
    {
        var parameters = new DialogParameters
        {
            ["PARId"] = _reportId!.Value,
            ["PARNumber"] = _model.PARNumber
        };

        var dialog = await DialogService.ShowAsync<ReturnPARDialog>("Return PAR Items", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await LoadReportAsync(_reportId.Value);
        }
    }

    private async Task CancelPARAsync()
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Cancellation",
            "Cancelling this PAR will reverse custodian assignments and mark it as cancelled. This action cannot be undone. Continue?",
            yesText: "Cancel PAR", cancelText: "Abort");

        if (confirmed == true && _reportId.HasValue)
        {
            try
            {
                await ApiClient.CancelPARAsync(new CancelPARCommand { Id = _reportId.Value });
                Snackbar.Add("PAR cancelled successfully", Severity.Success);
                await LoadReportAsync(_reportId.Value);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error cancelling PAR: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ResetWithConfirmation()
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Reset",
            "This will clear all unsaved changes. Continue?",
            yesText: "Reset", cancelText: "Cancel");

        if (confirmed == true)
        {
            if (_reportId.HasValue)
            {
                await LoadReportAsync(_reportId.Value);
            }
            else
            {
                _model = new PARModel { PARNumber = await GeneratePARNumberAsync(), IssuanceDate = DateTime.Today };
            }
        }
    }

    private void CancelEdit()
    {
        GoToList();
    }

    private void GoToList()
    {
        Navigation.NavigateTo("/inventories/reports/par-list");
    }

    private async Task PrintAsync()
    {
        if (_reportId.HasValue)
        {
            Navigation.NavigateTo($"/inventories/reports/par-print/{_reportId}");
        }
        else
        {
            Snackbar.Add("Please save the PAR first before printing", Severity.Warning);
        }
    }

    private class PARModel
    {
        public string PARNumber { get; set; } = "";
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public string Department { get; set; } = "";
        public string? Position { get; set; }
        public DateTime? IssuanceDate { get; set; }
        public string? IssuancePurpose { get; set; }
        public string? IssuanceLocation { get; set; }
        public string? Notes { get; set; }
        public string? IssuedByName { get; set; }
        public DateTime? IssuedByDate { get; set; }
        public string? ReceivedByName { get; set; }
        public DateTime? ReceivedByDate { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedByDate { get; set; }
        public List<PARLineItemModel> LineItems { get; set; } = new();
    }

    private class PARLineItemModel
    {
        public string PropertyCode { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DateAcquired { get; set; }
        public decimal AcquisitionCost { get; set; }
        public string? Condition { get; set; }
        public string? Remarks { get; set; }
    }
}
