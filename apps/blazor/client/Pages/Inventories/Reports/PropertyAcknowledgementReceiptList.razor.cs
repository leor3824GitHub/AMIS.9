using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.Reports;

public partial class PropertyAcknowledgementReceiptList
{
    [Inject] private IApiClient ApiClient { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private MudTable<PARListItemDto>? _table;
    private string? _searchString;
    private int? _statusFilter;
    private DateRange? _dateRange;
    private bool _loading;

    private async Task<TableData<PARListItemDto>> LoadDataAsync(TableState state)
    {
        _loading = true;
        try
        {
            var response = await ApiClient.GetPARListAsync(
                _searchString,
                _statusFilter,
                _dateRange?.Start,
                _dateRange?.End,
                state.Page + 1,
                state.PageSize);

            return new TableData<PARListItemDto>
            {
                Items = response.Data ?? Array.Empty<PARListItemDto>(),
                TotalItems = response.TotalCount
            };
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading PAR list: {ex.Message}", Severity.Error);
            return new TableData<PARListItemDto> { Items = Array.Empty<PARListItemDto>(), TotalItems = 0 };
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

    private Color GetStatusColor(PARStatus status)
    {
        return status switch
        {
            PARStatus.Draft => Color.Default,
            PARStatus.Posted => Color.Success,
            PARStatus.Returned => Color.Warning,
            PARStatus.Cancelled => Color.Error,
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
                await ApiClient.PostPARAsync(new PostPARCommand { Id = id });
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
