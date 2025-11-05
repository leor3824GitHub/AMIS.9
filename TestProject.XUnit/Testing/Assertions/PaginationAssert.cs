using AMIS.Framework.Core.Paging;

namespace TestProject.XUnit.Testing.Assertions;

public static class PaginationAssert
{
    public static void AssertDefaults<T>(PagedList<T>? payload)
        where T : class
    {
        Assert.NotNull(payload);
        Assert.Equal(1, payload!.PageNumber);
        Assert.Equal(10, payload.PageSize);
    }

    public static void AssertHasItems<T>(PagedList<T>? payload)
        where T : class
    {
        Assert.NotNull(payload);
        Assert.True(payload!.Items?.Count >= 1);
    }
}
