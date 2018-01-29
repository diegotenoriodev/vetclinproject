using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Validates through pre-defined regular expressions.
    /// Currently the available rules are:
    /// PhoneNumber,
    /// PostalCode,
    /// SIN, and
    /// Email
    /// </summary>
    public class RegexValidation : ValidationRule
    {
        public enum Rule
        {
            PhoneNumber,
            PostalCode,
            SIN,
            Email
        }

        private static Dictionary<Rule, string> rules = new Dictionary<Rule, string>()
        {
            { Rule.PhoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$" },
            { Rule.PostalCode, @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$" },
            { Rule.SIN, @"^(\d{3}-\d{3}-\d{3})$" },
            { Rule.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" }
        };

        public Rule ValidationRule { get; set; }
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                string str = value.ToString();
                if (!Regex.IsMatch(str, rules[ValidationRule]))
                {
                    return new ValidationResult(false, ErrorMessage);
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
