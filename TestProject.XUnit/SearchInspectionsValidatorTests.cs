using System;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using Xunit;

namespace AMIS.Tests.Catalog
{
    public class SearchInspectionsValidatorTests
    {
        [Fact]
        public void NullDates_AreValid()
        {
            var validator = new SearchInspectionsValidator();
            var cmd = new SearchInspectionsCommand
            {
                FromDate = null,
                ToDate = null
            };

            var result = validator.Validate(cmd);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OnlyFromDate_IsValid()
        {
            var validator = new SearchInspectionsValidator();
            var cmd = new SearchInspectionsCommand
            {
                FromDate = DateTime.UtcNow.AddDays(-7),
                ToDate = null
            };

            var result = validator.Validate(cmd);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void OnlyToDate_IsValid()
        {
            var validator = new SearchInspectionsValidator();
            var cmd = new SearchInspectionsCommand
            {
                FromDate = null,
                ToDate = DateTime.UtcNow
            };

            var result = validator.Validate(cmd);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ToDateBeforeFromDate_IsInvalid()
        {
            var validator = new SearchInspectionsValidator();
            var cmd = new SearchInspectionsCommand
            {
                FromDate = new DateTime(2025, 11, 2),
                ToDate = new DateTime(2025, 10, 31)
            };

            var result = validator.Validate(cmd);
            Assert.False(result.IsValid);
        }
    }
}
