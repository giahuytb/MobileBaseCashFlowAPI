using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class EditRequest
    {
        [Range(1, float.MaxValue, ErrorMessage = "Coin must be number and bigger than 0")]
        public float Coin { get; set; } = 0;


        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Out of range")]
        public float Point { get; set; } = 0;
    }
}
