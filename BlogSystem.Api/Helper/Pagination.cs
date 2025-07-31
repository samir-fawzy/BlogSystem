namespace BlogSystem.Api.Helper
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public Pagination(int pageSize,int pageIndex,int totalCount,IReadOnlyList<T> data) 
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Data = data;
        }
    }
}
