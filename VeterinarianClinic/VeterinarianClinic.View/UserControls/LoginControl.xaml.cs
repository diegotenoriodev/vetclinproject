using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// This class is responsible for responding to events from LoginControl.xaml.
    /// It uses the UserModel to validate the user, then the Login class to set shared
    /// information about the user.
    /// This information is used to control access.
    /// </summary>
    public partial class LoginControl : UserControl
    {
        /// <summary>
        /// This event is called when the authentication is succeed
        /// </summary>
        public event Action OnAuthenticated;

        /// <summary>
        /// Bound to the username textbox
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Has the logic for authentication and access to the repository.
        /// </summary>
        private UserModel userModel;

        public LoginControl()
        {
            InitializeComponent();
            gridLogin.DataContext = this;
            userModel = new UserModel();

            Validation.AddErrorHandler(txtUserName, ValidationError);
        }

        private List<string> errors = new List<string>();

        protected void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            string error = (string)e.Error.ErrorContent;

            if (!errors.Any(r => r.Equals(error)))
            {
                errors.Add(error);
                lblError.Content += $"- {error}\n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Validate the fields
            if (ValidateFields())
            {
                //AuthenticateUser returns user if it finds a user with the same password
                User user = userModel.AuthenticateUser(Username, txtPassword.Password);

                //If user is null, something is wrong.
                if (user == null)
                {
                    errors.Add("Username or password invalid!");
                }
                else
                {
                    //If a user is found, we create a login for that user and call the
                    //onAuthenticated to tell for those interested that user and password match.
                    Login.CreateLogin(user);
                    OnAuthenticated?.Invoke();
                }
            }

            if (errors.Any())
            {
                lblError.Content = UIHelper.GetStringFromList(errors);
            }
        }

        /// <summary>
        /// Validates the fields with specific rules and runs the rules from binding.
        /// </summary>
        /// <returns>returns true if no errors are found</returns>
        private bool ValidateFields()
        {
            //cleaning everything
            txtPassword.BorderBrush = null;
            lblError.Content = string.Empty;
            errors.Clear();

            //Executes the validation from the binding
            txtUserName.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            //Passwords cannot be bound, therefore, we cannot add a validation rule
            //Validation is done here
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                txtPassword.BorderBrush = Brushes.Red;
                errors.Add("Please inform the password!");
            }

            //If there are no errors, return true and everything is Okay!
            return !errors.Any();
        }

        /// <summary>
        /// Checks to identify when the user press enter.
        /// We expect the form to be submitted when enter is pressed.
        /// </summary>
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Button_Click(null, null);
            }
        }
    }
}
