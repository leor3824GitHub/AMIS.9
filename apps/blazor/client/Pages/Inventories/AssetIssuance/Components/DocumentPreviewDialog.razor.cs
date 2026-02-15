using System;
using System.Collections.Generic;
using System.Linq;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Inventories.AssetIssuance.Components;

public partial class DocumentPreviewDialog
{
    [Parameter]
    public IssuanceResponse? IssuanceData { get; set; }

    [Inject]
    private ISnackbar? Snackbar { get; set; }

    private IDialogReference? _dialog;

    public async Task OpenAsync()
    {
        _dialog = await DialogService.ShowAsync<MudDialog>("Document Preview", new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseButton = false
        });
    }

    private void Close()
    {
        _dialog?.Close(DialogResult.Cancel());
    }

    private async Task PrintDocument()
    {
        // Implementation for printing (uses JavaScript interop)
        Snackbar?.Add("Print functionality will open your browser's print dialog", Severity.Info);
        // In a real implementation, this would trigger JavaScript to open print preview
    }

    private Color GetDocTypeColor(IssuanceType? type) => type switch
    {
        IssuanceType._0 => Color.Warning,
        IssuanceType._1 => Color.Info,
        _ => Color.Default
    };

    private string GetDocTypeLabel(IssuanceType? type) => type switch
    {
        IssuanceType._0 => "PAR (Property Acknowledgement Receipt)",
        IssuanceType._1 => "ICS (Inventory Custodian Slip)",
        _ => "Unknown Document Type"
    };

    private string GetDocTypeAbbrev(IssuanceType? type) => type switch
    {
        IssuanceType._0 => "PAR",
        IssuanceType._1 => "ICS",
        _ => "Document"
    };

    private Color GetStatusColor(string? status) => status switch
    {
        "Pending" => Color.Warning,
        "Accepted" => Color.Success,
        "Rejected" => Color.Error,
        _ => Color.Default
    };

    private List<InventoryItemForIssuance> ConvertToPreviewItems()
    {
        return new();
    }
}

