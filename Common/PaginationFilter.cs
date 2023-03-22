namespace MobileBasedCashFlowAPI.Common
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            // minimum page number is always set to 1
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            // maximum page size a user can request for is 10
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
