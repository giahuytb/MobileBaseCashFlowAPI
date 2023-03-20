namespace MobileBasedCashFlowAPI.DTO
{
    public class GameReportRequest
    {
        public int ChildrenAmount { get; set; }
        public int TotalStep { get; set; }
        public double TotalMoney { get; set; }
        public bool IsWin { get; set; }
        public double Score { get; set; }
        public double IncomePerMonth { get; set; }
        public double ExpensePerMonth { get; set; }
    }
}
