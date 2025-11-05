using System;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;
using FluentValidation.TestHelper;

namespace TestProject.XUnit;

public class SearchInspectionRequestsValidatorTests
{
    private readonly SearchInspectionRequestsValidator _validator = new();

    [Fact]
    public void NullDates_AreValid()
    {
        var cmd = new SearchInspectionRequestsCommand();
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void OnlyFromDate_IsValid()
    {
        var cmd = new SearchInspectionRequestsCommand { FromDate = new DateTime(2024, 1, 1) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void OnlyToDate_IsValid()
    {
        var cmd = new SearchInspectionRequestsCommand { ToDate = new DateTime(2024, 1, 31) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ValidRange_IsValid()
    {
        var cmd = new SearchInspectionRequestsCommand { FromDate = new DateTime(2024, 1, 1), ToDate = new DateTime(2024, 1, 31) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void InvalidRange_HasError()
    {
        var cmd = new SearchInspectionRequestsCommand { FromDate = new DateTime(2024, 2, 1), ToDate = new DateTime(2024, 1, 1) };
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("ToDate must be greater than or equal to FromDate.");
    }
}
