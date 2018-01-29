using System.Globalization;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Validates if a numeric value is between a range
    /// </summary>
    public class RangeValidation : ValidationRule
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public string ErrorMessage { get; set; }

        public RangeValidation()
        {
            Min = 0;
            Max = 9999;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                if(decimal.TryParse(value.ToString(), out decimal val))
                {
                    if(val <= Min || val >= Max)
                    {
                        return new ValidationResult(false, ErrorMessage);
                    }
                }
                else
                {
                    return new ValidationResult(false, ErrorMessage);
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
