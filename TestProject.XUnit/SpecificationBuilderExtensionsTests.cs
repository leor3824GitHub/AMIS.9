using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using Ardalis.Specification;

namespace TestProject.XUnit;

public class SpecificationBuilderExtensionsTests
{
    private class Dummy
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public DummyCategory? Category { get; set; }
    }

    private class DummyCategory
    {
        public string? Name { get; set; }
    }

    private class OrderBySpec : Specification<Dummy>
    {
        public OrderBySpec(string[] orderBy)
        {
            Query.OrderBy(orderBy);
        }
    }

    private class PaginateSpec : Specification<Dummy>
    {
        public PaginateSpec(PaginationFilter filter)
        {
            Query.PaginateBy(filter);
        }
    }

    [Fact]
    public void OrderBy_ParsesMultipleFields_WithCorrectOrderingTypes()
    {
        var spec = new OrderBySpec(new[] { "Name", "Price Desc", "Category.Name Desc" });

        Assert.Equal(3, spec.OrderExpressions.Count());

        var orderList = spec.OrderExpressions.ToList();
        
        // First should be OrderBy
        Assert.Equal(OrderTypeEnum.OrderBy, orderList[0].OrderType);
        // Then ThenByDescending
        Assert.Equal(OrderTypeEnum.ThenByDescending, orderList[1].OrderType);
        // Then ThenByDescending again
        Assert.Equal(OrderTypeEnum.ThenByDescending, orderList[2].OrderType);
    }

    [Fact]
    public void PaginateBy_Normalizes_DefaultPageNumberAndPageSize()
    {
        var filter = new PaginationFilter { PageNumber = 0, PageSize = 0 };
        _ = new PaginateSpec(filter);

        Assert.Equal(1, filter.PageNumber);
        Assert.Equal(10, filter.PageSize);
    }
}
