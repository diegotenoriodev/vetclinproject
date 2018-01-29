using System;
using System.Globalization;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Identify if a date is higher than today
    /// </summary>
    public class MaxDateValue : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public MaxDateValue()
        {
            ErrorMessage = "Date cannot be bigger than today.";
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value != null)
            {
                if(value is DateTime)
                {
                    DateTime dt = (DateTime)value;

                    if(dt > DateTime.Now.Date)
                    {
                        return new ValidationResult(false, ErrorMessage);
                    }
                }
            }

            return ValidationResult.ValidResult;

        }
    }
}
