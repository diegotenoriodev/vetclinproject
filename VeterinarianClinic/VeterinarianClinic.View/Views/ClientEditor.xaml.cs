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
    /// Interaction logic for ClientEditor.xaml
    /// </summary>
    public partial class ClientEditor : UserControl, IEditor<Client>
    {
        private Client client;
        public Client Entity
        {
            get
            {
                return client;
            }
            set
            {
                CleanComponents();
                client = value;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = client.Clone();
            }
        }

        private ClientModel clientModel;

        private List<string> errors = new List<string>();

        public event Action OnSave;

        public event Action OnCancel;

        public ClientEditor()
        {
            InitializeComponent();
            Entity = new Client();
            clientModel = new ClientModel();

            Validation.AddErrorHandler(txtName, ValidationError);
            Validation.AddErrorHandler(txtPhoneNumber, ValidationError);
            Validation.AddErrorHandler(txtSIN, ValidationError);
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

        public void Clean()
        {
            Entity = new Client();
        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;
        }

        private bool ExecuteValidations()
        {
            txtName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtPhoneNumber.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtSIN.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            return !errors.Any();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            errors.Clear();

            if (ExecuteValidations())
            {
                //Although we work binding an object to the XAML, we did not want to give 
                //to the view for edit the actual object being changed, so when we set 
                //the Entity, the datacontext is loaded with a copy of the object 
                //and when the user finally saves it, we set the properties
                //of our entity (that is the one shown in the grid) with the values from the
                //view (DataContext).
                Client changedClient = (Client)fieldsGrid.DataContext;

                client.Id = changedClient.Id;
                client.Name = changedClient.Name;
                client.PhoneNumber = changedClient.PhoneNumber;
                client.SIN = changedClient.SIN;

                //It could be bound through the interface, but what I want to do is to allow that the errors are shown here
                ResultOperation result = clientModel.Save(Entity);

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

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Clean();
            OnCancel?.Invoke();
        }

        public void Reload()
        {
        }
    }
}
