namespace MobileBasedCashFlowAPI.DTO
{
    public class FinancialReportRequest
    {
        public int ChildrenAmount { get; set; }
        public double IncomePerMonth { get; set; }
        public double ExpensePerMonth { get; set; }
    }
}
