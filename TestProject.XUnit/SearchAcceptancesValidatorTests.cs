using System;
using AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;
using FluentValidation.TestHelper;

namespace TestProject.XUnit;

public class SearchAcceptancesValidatorTests
{
    private readonly SearchAcceptancesValidator _validator = new();

    [Fact]
    public void NullDates_AreValid()
    {
        var cmd = new SearchAcceptancesCommand();
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void OnlyFromDate_IsValid()
    {
        var cmd = new SearchAcceptancesCommand { FromDate = new DateTime(2024, 1, 1) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void OnlyToDate_IsValid()
    {
        var cmd = new SearchAcceptancesCommand { ToDate = new DateTime(2024, 1, 31) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ValidRange_IsValid()
    {
        var cmd = new SearchAcceptancesCommand { FromDate = new DateTime(2024, 1, 1), ToDate = new DateTime(2024, 1, 31) };
        var result = _validator.TestValidate(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void InvalidRange_HasError()
    {
        var cmd = new SearchAcceptancesCommand { FromDate = new DateTime(2024, 2, 1), ToDate = new DateTime(2024, 1, 1) };
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("ToDate must be greater than or equal to FromDate.");
    }
}
