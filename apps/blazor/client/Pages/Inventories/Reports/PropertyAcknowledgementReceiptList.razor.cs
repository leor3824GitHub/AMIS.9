using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PropertyAcknowledgementReceiptList
{
    [Inject] private IApiClient ApiClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private MudTable<PARDto>? _table;
    private string? _searchString;
    private string? _statusFilter;
    private DateRange? _dateRange;
    private bool _loading;

    private async Task<TableData<PARDto>> LoadDataAsync(TableState state, CancellationToken cancellationToken)
    {
        _loading = true;
        try
        {
            var response = await ApiClient.GetPARListAsync();

            // Client-side filtering (since the API endpoint doesn't support server-side filtering)
            var filtered = response?.PaRs ?? new List<PARDto>();

            if (!string.IsNullOrEmpty(_searchString))
            {
                filtered = filtered.Where(p =>
                    p.ParNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                    p.EmployeeName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
            }

            if (!string.IsNullOrEmpty(_statusFilter))
            {
                filtered = filtered.Where(p => p.Status == _statusFilter).ToList();
            }

            if (_dateRange?.Start.HasValue == true || _dateRange?.End.HasValue == true)
            {
                filtered = filtered.Where(p =>
                    (!_dateRange.Start.HasValue || p.IssuanceDate >= _dateRange.Start.Value) &&
                    (!_dateRange.End.HasValue || p.IssuanceDate <= _dateRange.End.Value)
                ).ToList();
            }

            // Sort by date descending by default
            var sorted = filtered.OrderByDescending(p => p.IssuanceDate).ToList();

            // Pagination
            var paginated = sorted
                .Skip(state.Page * state.PageSize)
                .Take(state.PageSize)
                .ToList();

            return new TableData<PARDto>
            {
                Items = paginated,
                TotalItems = sorted.Count
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading PAR list: {ex.Message}", Severity.Error);
            return new TableData<PARDto> { Items = Array.Empty<PARDto>(), TotalItems = 0 };
        }
        finally
        {
            _loading = false;
        }
    }

    private void ClearFilters()
    {
        _searchString = null;
        _statusFilter = null;
        _dateRange = null;
        _table?.ReloadServerData();
    }

    private Color GetStatusColor(string status)
    {
        return status?.ToLower() switch
        {
            "draft" => Color.Default,
            "posted" => Color.Success,
            "returned" => Color.Warning,
            "cancelled" => Color.Error,
            _ => Color.Default
        };
    }

    private async Task PostPAR(Guid id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Post",
            "Posting this PAR will assign the PPE items to the custodian and lock the document. Continue?",
            yesText: "Post", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                await ApiClient.PostPARAsync(id);
                Snackbar.Add("PAR posted successfully", Severity.Success);
                await _table!.ReloadServerData();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error posting PAR: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ReturnPAR(Guid id, string parNumber)
    {
        var parameters = new DialogParameters
        {
            ["PARId"] = id,
            ["PARNumber"] = parNumber
        };

        var dialog = await DialogService.ShowAsync<ReturnPARDialog>("Return PAR Items", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table!.ReloadServerData();
        }
    }
}
