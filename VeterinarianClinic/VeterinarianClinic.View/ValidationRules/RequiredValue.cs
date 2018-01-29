using System.Globalization;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Identifies when a value is required.
    /// It verifies if a value is null, empty or if a user input only spaces.
    /// It also presents the property MinLength, so the developer can specify what is the min length that
    /// makes a value be considered not null.
    /// </summary>
    public class RequiredValue : ValidationRule
    {
        public int MinLength { get; set; }
        public string ErrorMessage { get; set; }

        public RequiredValue()
        {
            MinLength = 0;
            ErrorMessage = "Please select a valid value";
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, ErrorMessage);
            }
            else if(value is string) //These rules are only applied when the object is a string
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return new ValidationResult(false, ErrorMessage);
                }
                else if (string.IsNullOrEmpty(value.ToString().Trim()))
                {
                    return new ValidationResult(false, ErrorMessage);
                }
                else if (value.ToString().Trim().Length <= MinLength)
                {
                    return new ValidationResult(false, ErrorMessage);
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
