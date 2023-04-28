using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.DTO
{
    public class GameReportRequest
    {
        [Required(ErrorMessage = "Please enter asset price")]
        [Range(0, 3, ErrorMessage = "Price must be a mumber and from 0 - 3")]
        public int ChildrenAmount { get; set; }

        [Required(ErrorMessage = "Please enter total step")]
        [Range(1, int.MaxValue, ErrorMessage = "Total step must be a mumber and bigger than 0")]
        public int TotalStep { get; set; }

        [Required(ErrorMessage = "Please enter total money")]
        [Range(1, double.MaxValue, ErrorMessage = "Total money must be a mumber and bigger than 0")]
        public double TotalMoney { get; set; }

        [Required(ErrorMessage = "Please enter true or false")]
        public bool IsWin { get; set; }

        [Required(ErrorMessage = "Please enter asset price")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a mumber and bigger than 0")]
        public double Score { get; set; }

        [Required(ErrorMessage = "Please enter income per month")]
        [Range(1, double.MaxValue, ErrorMessage = "Income per month must be a mumber and bigger than 0")]
        public double IncomePerMonth { get; set; }

        [Required(ErrorMessage = "Please enter expense per month")]
        [Range(1, double.MaxValue, ErrorMessage = "Expense per month must be a mumber and bigger than 0")]
        public double ExpensePerMonth { get; set; }
    }
}
