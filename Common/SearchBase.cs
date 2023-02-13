namespace MobileBaseCashFlowGameAPI.Common
{
    public class SearchBase
    {
        public SearchBase(string search, double? from, double? to, string sort)
        {
            search = Search;
            from = From;
            to = To;
            sort = Sort;
        }

        public string Search { get; set; } = string.Empty;
        public double From { get; set; } = double.MaxValue;
        public double To { get; set; } = double.MaxValue;
        public string Sort { get; set; } = string.Empty;
    }
}
