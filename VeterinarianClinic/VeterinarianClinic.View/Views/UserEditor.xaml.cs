using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// A basic user control that binds a property to the controls in the xaml.
    /// It implements the IEditor for User, that specify a set of operations that it has to define.
    /// </summary>
    public partial class UserEditor : UserControl, IEditor<User>
    {
        private User user;
        public User Entity
        {
            get
            {
                return user;
            }
            set
            {
                //The set should clean the compontents (reset errors and so on)
                CleanComponents();
                user = value;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = user.Clone();

                //This validation depends on the new UserId to execute the validation
                usernameValidation.UserId = Entity.Id;
            }
        }
        
        /// <summary>
        /// The implementation of userModel
        /// </summary>
        private UserModel userModel;

        List<string> errors = new List<string>();

        public event Action OnSave;
        public event Action OnCancel;

        public void Clean()
        {
            Entity = new User();
            CleanComponents();
        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;
            txtConfPass.Password = string.Empty;
            txtPassword.Password = string.Empty;
            txtPassword.BorderBrush = System.Windows.Media.Brushes.Gray;
            txtConfPass.BorderBrush = System.Windows.Media.Brushes.Gray;
        }

        public UserEditor()
        {
            InitializeComponent();

            //Starts the Entity, Usermodel, sets to the validation the userModel that will 
            //be used to retrieve users and execute the rule.
            Entity = new User();
            userModel = new UserModel();
            usernameValidation.userModel = userModel;

            Loaded += UserEditor_Loaded;

            Validation.AddErrorHandler(txtName, ValidationError);
            Validation.AddErrorHandler(txtUsername, ValidationError);
            Validation.AddErrorHandler(cbxUserType, ValidationError);
        }

        protected void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            string error = (string)e.Error.ErrorContent;

            if (!errors.Any(r => r.Equals(error)))
            {
                errors.Add(error);
                lblErrors.Content += $"- {error}\n";
            }
        }

        private void UserEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //Load the list from the UsersType defined in our database
            cbxUserType.ItemsSource = userModel.GetUsersType();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            //Reset the error list
            errors.Clear();

            //Execute the validations
            if (ExecuteValidations())
            {
                //Password is not bound for safety issues, therefore we have to do it by code
                if (!string.IsNullOrEmpty(txtPassword.Password))
                {
                    Entity.ChangePassword = true;
                    Entity.Password = txtPassword.Password;
                }

                //Although we work binding an object to the XAML, we did not want to give 
                //to the view for edit the actual object being changed, so when we set 
                //the Entity, the datacontext is loaded with a copy of the object 
                //and when the user finally saves it, we set the properties
                //of our entity (that is the one shown in the grid) with the values from the
                //view (DataContext).
                User changedUser = (User)fieldsGrid.DataContext;

                Entity.Id = changedUser.Id;
                Entity.IdUserType = changedUser.IdUserType;
                Entity.UserType = changedUser.UserType;
                Entity.Name = changedUser.Name;
                Entity.Username = changedUser.Username;

               //It could be bound through the interface, but what I want to do is to 
               //allow that the errors are shown here
               ResultOperation result = userModel.Save(Entity);

                if (result.Success)
                {
                    OnSave?.Invoke();
                }
                else
                {
                    errors.AddRange(result.Errors);
                }
            }

            if (errors.Any())
            {
                lblErrors.Content = UIHelper.GetStringFromList(errors);
            }
        }

        private bool ValidatePassword()
        {
            //As there are no bounds for controls type Password, we have to do it here.
            string error = null;
            txtPassword.BorderBrush = System.Windows.Media.Brushes.Gray;
            txtConfPass.BorderBrush = System.Windows.Media.Brushes.Gray;

            //Password is only mandatory for new users
            if (Entity.Id == 0)
            {
                if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    error = "Password is mandatory!";
                    txtPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else if (string.IsNullOrEmpty(txtConfPass.Password))
                {
                    error = "Password Confirmation is mandatory!";
                    txtConfPass.BorderBrush = System.Windows.Media.Brushes.Red;
                }
            }

            //If the validation did not fall for the previous rules 
            if (string.IsNullOrEmpty(error))
            {
                if (!string.IsNullOrEmpty(txtPassword.Password) && string.IsNullOrEmpty(txtConfPass.Password))
                {
                    error = "Password confirmation is mandatory!";
                    txtConfPass.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else if (string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtConfPass.Password))
                {
                    error = "Password is mandatory!";
                    txtPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else if (!txtConfPass.Password.Equals(txtPassword.Password))
                {
                    error = "Password and Confirmation do not match!";
                    txtPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                    txtConfPass.BorderBrush = System.Windows.Media.Brushes.Red;
                }
            }
            
            if (string.IsNullOrEmpty(error))
            {
                return true;
            }

            AddError(error);
            return false;
        }

        private bool ExecuteValidations()
        {
            bool returnVal = true;

            if (!ValidatePassword()) { returnVal = false; }

            txtName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtUsername.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            cbxUserType.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            if (errors.Any()) { returnVal = false; }

            return returnVal;
        }

        private void AddError(string str)
        {
            errors.Add(str);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Clean();
            OnCancel?.Invoke();
        }

        public void Reload()
        {
            UserEditor_Loaded(null, null);
        }
    }
}