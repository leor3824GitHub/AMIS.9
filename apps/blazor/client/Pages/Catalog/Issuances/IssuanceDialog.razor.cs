using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Infrastructure.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public partial class IssuanceDialog
{
    [Inject]
    private IApiClient ApiClient { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public IssuanceEditModel Model { get; set; } = default!;

    [Parameter]
    public bool? IsCreate { get; set; }

    [Parameter]
    public IReadOnlyList<EmployeeResponse> Employees { get; set; } = Array.Empty<EmployeeResponse>();
    [Parameter]
    public IReadOnlyList<ProductResponse> Products { get; set; } = Array.Empty<ProductResponse>();

    private FshValidation? _customValidation;
    private Guid? SelectedEmployeeId;
    private DateTime _issuanceDate = DateTime.Today;

    protected override void OnParametersSet()
    {
        if (Model != null && Model.EmployeeId != Guid.Empty)
        {
            SelectedEmployeeId = Model.EmployeeId;
        }
        else if (Employees.Count > 0)
        {
            SelectedEmployeeId = Employees[0].Id;
        }
        else
        {
            SelectedEmployeeId = null;
        }

        if (Model != null)
        {
            _issuanceDate = Model.IssuanceDate;
        }
    }

    private async Task OnValidSubmit()
    {
        if (IsCreate == null)
        {
            return;
        }

        if (SelectedEmployeeId is null)
        {
            Snackbar.Add("Please select an employee.", Severity.Warning);
            return;
        }

        Model.EmployeeId = SelectedEmployeeId.Value;

        if (IsCreate.Value)
        {
            // Use aggregate method for create with items
            var aggregateCommand = new UpdateIssuanceWithItemsCommand
            {
                Id = Guid.NewGuid(),
                EmployeeId = Model.EmployeeId,
                IssuanceDate = Model.IssuanceDate,
                TotalAmount = (double)Model.TotalAmount,
                IsClosed = Model.IsClosed,
                Items = Model.Items?.Select(item => new IssuanceItemUpsert
                {
                    ProductId = item.ProductId!.Value,
                    Qty = item.Qty,
                    UnitPrice = item.UnitPrice,
                    Status = item.Status
                }).ToList() ?? new List<IssuanceItemUpsert>(),
                DeletedItemIds = new List<Guid>()
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateIssuanceWithItemsEndpointAsync("1", aggregateCommand.Id, aggregateCommand),
                Snackbar,
                Navigation,
                _customValidation);

            if (response != null)
            {
                Snackbar.Add("Issuance created successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            // For update, we need to get existing items and determine what to update/delete
            var existingItems = await ApiClient.SearchIssuanceItemsEndpointAsync("1", new SearchIssuanceItemsCommand
            {
                IssuanceId = Model.Id,
                PageNumber = 1,
                PageSize = 1000
            });

            var itemsToUpdate = new List<IssuanceItemUpsert>();
            var deletedItemIds = new List<Guid>();

            // Process existing items
            if (existingItems?.Items != null)
            {
                foreach (var existing in existingItems.Items)
                {
                    if (!existing.Id.HasValue) continue;

                    var currentInput = Model.Items?.FirstOrDefault(i => i.Id == existing.Id);
                    if (currentInput != null)
                    {
                        // Update existing item
                        itemsToUpdate.Add(new IssuanceItemUpsert
                        {
                            Id = existing.Id,
                            ProductId = currentInput.ProductId!.Value,
                            Qty = currentInput.Qty,
                            UnitPrice = currentInput.UnitPrice,
                            Status = currentInput.Status
                        });
                    }
                    else
                    {
                        // Mark for deletion
                        deletedItemIds.Add(existing.Id.Value);
                    }
                }
            }

            // Add new items
            if (Model.Items != null)
            {
                foreach (var newItem in Model.Items.Where(i => i.Id == Guid.Empty))
                {
                    itemsToUpdate.Add(new IssuanceItemUpsert
                    {
                        ProductId = newItem.ProductId!.Value,
                        Qty = newItem.Qty,
                        UnitPrice = newItem.UnitPrice,
                        Status = newItem.Status
                    });
                }
            }

            var aggregateCommand = new UpdateIssuanceWithItemsCommand
            {
                Id = Model.Id,
                EmployeeId = Model.EmployeeId,
                IssuanceDate = Model.IssuanceDate,
                TotalAmount = (double)Model.TotalAmount,
                IsClosed = Model.IsClosed,
                Items = itemsToUpdate,
                DeletedItemIds = deletedItemIds
            };

            var response = await ApiHelper.ExecuteCallGuardedAsync(
                () => ApiClient.UpdateIssuanceWithItemsEndpointAsync("1", Model.Id, aggregateCommand),
                Snackbar,
                Navigation,
                _customValidation);

            if (response != null)
            {
                Snackbar.Add("Issuance updated successfully!", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private void OnDateChanged(DateTime? value)
    {
        if (value.HasValue)
        {
            _issuanceDate = value.Value;
            Model.IssuanceDate = value.Value;
        }
    }

    private void UpdateTotalAmount(double total)
    {
        Model.TotalAmount = Convert.ToDecimal(total);
        StateHasChanged();
    }
}
