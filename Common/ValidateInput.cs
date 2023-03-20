using System.Text.RegularExpressions;

namespace MobileBasedCashFlowAPI.Common
{
    public class ValidateInput
    {
        // Validate string
        private static Regex regexName = new Regex("^[a-zA-Z ]*$");
        // Validate number phone of VN
        private static Regex regexPhone = new Regex(@"(84|0[3|5|7|8|9])+([0-9]{8})\b");
        // Validate should be character or number
        private static Regex regexSpecialChar = new Regex("[^a-zA-Z0-9]");
        // Validate number
        private static Regex regexNuber = new Regex(@"^-?[0-9]\d*(\.\d+)?$");
        // Validarte min max
        private static Regex hasMiniMaxChars = new Regex(@".{6,30}");
        // Validate symbols
        private static Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        // Validate num symbol
        private static Regex regexNumSymbol = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]*$");
        // Validate Email
        private static Regex regexEmail = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");

        public static bool isName(string s)
        {
            if (regexName.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isPhone(string s)
        {
            if (regexPhone.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isSpecialChar(string s)
        {
            if (regexSpecialChar.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isNumber(string s)
        {
            if (regexNuber.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isMiniMaxChars(string s)
        {
            if (hasMiniMaxChars.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isSymbols(string s)
        {
            if (hasSymbols.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static bool isNumSymbol(string s)
        {
            if (regexNumSymbol.IsMatch(s))
            {
                return true;
            }
            return false;
        }
        public static bool isEmail(string s)
        {
            if (regexEmail.IsMatch(s))
            {
                return true;
            }
            return false;
        }

        public static int getAge(DateTime bornDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - bornDate.Year;
            if (bornDate > today.AddYears(-age))
                age--;
            return age;
        }
    }
}
