using AMIS.Framework.Core.Paging;

namespace TestProject.XUnit.Testing.Paging;

public static class TestPagedList
{
    public static PagedList<T> Build<T>(IEnumerable<T> items, int pageNumber, int pageSize, int? totalCount = null)
        where T : class
    {
        var list = items.ToList();
        var pn = pageNumber <= 0 ? 1 : pageNumber;
        var ps = pageSize <= 0 ? 10 : pageSize;
        var tc = totalCount ?? list.Count;
        return new PagedList<T>(list, pn, ps, tc);
    }
}
