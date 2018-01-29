using System;
using System.Globalization;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    public class MinDateValue : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public MinDateValue()
        {
            ErrorMessage = "Date cannot be before tomorow.";
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value != null)
            {
                if(value is DateTime)
                {
                    DateTime dt = (DateTime)value;

                    if(dt < DateTime.Now.Date.AddHours(1))
                    {
                        return new ValidationResult(false, ErrorMessage);
                    }
                }
            }

            return ValidationResult.ValidResult;

        }
    }
}
