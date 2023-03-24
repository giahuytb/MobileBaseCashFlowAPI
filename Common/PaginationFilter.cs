namespace MobileBasedCashFlowAPI.Common
{
    public class PaginationFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageIndex, int pageSize)
        {
            // minimum page number is always set to 1
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            // maximum page size a user can request for is 10
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
