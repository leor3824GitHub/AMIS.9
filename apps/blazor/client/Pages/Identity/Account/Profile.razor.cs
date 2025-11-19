using AMIS.Blazor.Client.Components;
using AMIS.Blazor.Client.Components.Dialogs;
using AMIS.Blazor.Infrastructure.Api;
using AMIS.Blazor.Infrastructure.Auth;
using AMIS.Blazor.Shared.Employees;
using AMIS.Shared.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Linq;

namespace AMIS.Blazor.Client.Pages.Identity.Account;

public partial class Profile
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject]
    protected IAuthenticationService AuthService { get; set; } = default!;
    [Inject]
    protected IApiClient PersonalClient { get; set; } = default!;

    private readonly UpdateUserCommand _profileModel = new();
    private EmployeeResponse? _employeeProfile;

    private string? _imageUrl;
    private string? _userId;
    private char _firstLetterOfName;
    private bool _hasEmployeeProfile;
    private bool _isLoadingEmployee;

    private FshValidation? _customValidation;

    protected override async Task OnInitializedAsync()
    {
        if ((await AuthState).User is { } user)
        {
            _userId = user.GetUserId();
            _profileModel.Email = user.GetEmail() ?? string.Empty;
            _profileModel.FirstName = user.GetFirstName() ?? string.Empty;
            _profileModel.LastName = user.GetSurname() ?? string.Empty;
            _profileModel.PhoneNumber = user.GetPhoneNumber();
            if (user.GetImageUrl() != null)
            {
                _imageUrl = user.GetImageUrl()!.ToString();
            }
            if (_userId is not null) _profileModel.Id = _userId;

            // Load employee profile
            await LoadEmployeeProfileAsync();
            
            // Auto-create employee record if profile is complete but no employee exists
            if (!_hasEmployeeProfile && IsProfileComplete())
            {
                await AutoCreateEmployeeRecordAsync();
            }
        }

        if (_profileModel.FirstName?.Length > 0)
        {
            _firstLetterOfName = _profileModel.FirstName.ToUpperInvariant().FirstOrDefault();
        }
    }
    
    private bool IsProfileComplete()
    {
        return !string.IsNullOrWhiteSpace(_profileModel.FirstName) && 
               !string.IsNullOrWhiteSpace(_profileModel.LastName);
    }
    
    private async Task AutoCreateEmployeeRecordAsync()
    {
        try
        {
            var fullName = $"{_profileModel.FirstName} {_profileModel.LastName}".Trim();
            
            var selfRegisterCommand = new SelfRegisterEmployeeCommand
            {
                Name = fullName,
                Designation = "Employee",
                ResponsibilityCode = "GENERAL"
            };

            var result = await PersonalClient.SelfRegisterEmployeeEndpointAsync("1", selfRegisterCommand);
            
            if (result?.Id != Guid.Empty)
            {
                await LoadEmployeeProfileAsync(); // Reload to show new employee info
                Toast.Add("Employee record created automatically!", Severity.Success);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Auto-creation failed: {ex.Message}");
            // Silent fail - UI will show manual registration option
        }
    }

    private async Task LoadEmployeeProfileAsync()
    {
        if (string.IsNullOrEmpty(_userId))
            return;

        try
        {
            _isLoadingEmployee = true;
            StateHasChanged();

            var filter = new SearchEmployeesCommand
            {
                PageNumber = 1,
                PageSize = 1,
                UsrId = Guid.Parse(_userId)
            };

            var response = await PersonalClient.SearchEmployeesEndpointAsync("1", filter);
            _employeeProfile = response?.Items?.FirstOrDefault();
            _hasEmployeeProfile = _employeeProfile != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load employee profile: {ex.Message}");
            _hasEmployeeProfile = false;
        }
        finally
        {
            _isLoadingEmployee = false;
            StateHasChanged();
        }
    }

    private async Task UpdateProfileAsync()
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => PersonalClient.UpdateUserEndpointAsync(_profileModel), Toast, _customValidation))
        {
            // If employee profile exists, sync basic contact info
            if (_hasEmployeeProfile && _employeeProfile != null)
            {
                await SyncEmployeeBasicInfoAsync();
            }
            // If no employee profile and profile is now complete, auto-create employee record
            else if (IsProfileComplete())
            {
                await AutoCreateEmployeeRecordAsync();
            }

            Toast.Add("Your Profile has been updated. Please Login again to Continue.", Severity.Success);
            await AuthService.ReLoginAsync(Navigation.Uri);
        }
    }

    private async Task SyncEmployeeBasicInfoAsync()
    {
        if (_employeeProfile == null)
            return;

        try
        {
            // Sync only basic info from Identity to Employee
            var updateEmployeeCommand = new UpdateEmployeeCommand
            {
                Id = _employeeProfile.Id ?? Guid.Empty,
                Name = $"{_profileModel.FirstName} {_profileModel.LastName}",
                // Keep business fields unchanged
                Designation = _employeeProfile.Designation ?? string.Empty,
                ResponsibilityCode = _employeeProfile.ResponsibilityCode ?? string.Empty,
                UserId = _employeeProfile.UserId
            };

            await PersonalClient.UpdateEmployeeEndpointAsync(
                "1",
                _employeeProfile.Id ?? Guid.Empty,
                updateEmployeeCommand);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to sync employee profile: {ex.Message}");
            // Don't fail the whole operation - Identity update succeeded
        }
    }

    private void NavigateToEmployeeRegistration()
    {
        Navigation.NavigateTo("/catalog/employees");
    }

    private void NavigateToEmployeeEdit()
    {
        if (_employeeProfile?.Id != null)
        {
            Navigation.NavigateTo($"/catalog/employees");
        }
    }



    private async Task ManualEmployeeRegistrationAsync()
    {
        try
        {
            var fullName = $"{_profileModel.FirstName} {_profileModel.LastName}".Trim();
            
            if (string.IsNullOrEmpty(fullName))
            {
                Toast.Add("Please complete your profile name before registering as an employee.", Severity.Warning);
                return;
            }

            var selfRegisterCommand = new SelfRegisterEmployeeCommand
            {
                Name = fullName,
                Designation = "Employee",
                ResponsibilityCode = "GENERAL"
            };

            var result = await PersonalClient.SelfRegisterEmployeeEndpointAsync("1", selfRegisterCommand);
            
            if (result?.Id != Guid.Empty)
            {
                Toast.Add("Employee registration successful!", Severity.Success);
                await LoadEmployeeProfileAsync();
            }
        }
        catch (Exception ex)
        {
            Toast.Add($"Registration failed: {ex.Message}", Severity.Error);
        }
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file is not null)
        {
            string? extension = Path.GetExtension(file.Name);
            if (!AppConstants.SupportedImageFormats.Any(f => string.Equals(f, extension, StringComparison.OrdinalIgnoreCase)))
            {
                Toast.Add("Image Format Not Supported.", Severity.Error);
                return;
            }

            string? fileName = $"{_userId}-{Guid.NewGuid():N}";
            fileName = fileName[..Math.Min(fileName.Length, 90)];
            var imageFile = await file.RequestImageFileAsync(AppConstants.StandardImageFormat, AppConstants.MaxImageWidth, AppConstants.MaxImageHeight);
            byte[]? buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream(AppConstants.MaxAllowedSize).ReadExactlyAsync(buffer);
            string? base64String = $"data:{AppConstants.StandardImageFormat};base64,{Convert.ToBase64String(buffer)}";
            _profileModel.Image = new FileUploadCommand() { Name = fileName, Data = base64String, Extension = extension };

            await UpdateProfileAsync();
        }
    }

    public async Task RemoveImageAsync()
    {
        string deleteContent = "You're sure you want to delete your Profile Image?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), deleteContent }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            _profileModel.DeleteCurrentImage = true;
            await UpdateProfileAsync();
        }
    }
}
