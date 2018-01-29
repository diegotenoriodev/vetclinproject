using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using VeterinarianClinic.Model;

namespace VeterinarianClinic.View.ValidationRules
{
    /// <summary>
    /// Verifies if a username is available
    /// </summary>
    class UsernameValidation : ValidationRule
    {
        public UserModel userModel { get; set; }
        public int UserId { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value != null)
            {
                if(userModel == null)
                {
                    userModel = new UserModel();
                }

                var query = from r in userModel.GetAll(string.Empty)
                            where r.Username.ToLower() == value.ToString().ToLower()
                            && r.Id != UserId
                            select r;

                if (query.Any())
                {
                    return new ValidationResult(false, "Username already exists in our system. Please, choose another one.");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
