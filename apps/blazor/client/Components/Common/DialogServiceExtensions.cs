using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMIS.Blazor.Client.Components.Common;

public static class DialogServiceExtensions
{
    public static Task<IDialogReference> ShowModalAsync<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
        return dialogService.ShowAsync<TDialog>(string.Empty, parameters, options);
    }

    // Kept for backward compatibility. Prefer ShowModalAsync.
    [Obsolete("Use ShowModalAsync instead. This will be removed in a future version.")]
    public static IDialogReference ShowModal<TDialog>(this IDialogService dialogService, DialogParameters parameters)
        where TDialog : ComponentBase
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };
        return dialogService.Show<TDialog>(string.Empty, parameters, options);
    }
}