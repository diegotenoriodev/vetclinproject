using System.Globalization;
using System.Windows.Controls;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Identify if a field presents at least determined number of words.
    /// It is used in name for instance, in our application, a client or doctor must
    /// have at least first and last name.
    /// </summary>
    class QuantityOfWords : ValidationRule
    {
        public int Qtd { get; set; }
        public string ErrorMessage { get; set; }

        public QuantityOfWords()
        {
            Qtd = 2;
            ErrorMessage = string.Empty;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                string str = value.ToString();

                if (str.Trim().Split(' ').Length < Qtd)
                {
                    if(ErrorMessage == string.Empty)
                    {
                        ErrorMessage = $"The field should have at least {Qtd} words";
                    }

                    return new ValidationResult(false, ErrorMessage);
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
