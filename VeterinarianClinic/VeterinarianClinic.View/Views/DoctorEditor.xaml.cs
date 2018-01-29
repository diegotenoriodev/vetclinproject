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
    /// Interaction logic for DoctorEditor.xaml
    /// </summary>
    public partial class DoctorEditor : UserControl, IEditor<Doctor>
    {
        private DoctorModel docModel;

        List<string> errors = new List<string>();
        
        public DoctorEditor()
        {
            InitializeComponent();
            

            Entity = new Doctor();
            docModel = new DoctorModel();

            //Loaded += DoctorEditor_Loaded;

            Validation.AddErrorHandler(txtName, ValidationError);
            Validation.AddErrorHandler(txtEmail, ValidationError);
            Validation.AddErrorHandler(txtPhoneNumber, ValidationError);
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
       
        private Doctor doctor;
        public Doctor Entity
        {
            get
            {
                return doctor;
            }
            set
            {
                CleanComponents();
                doctor = value;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = doctor.Clone();
            }
        }
        


        public event Action OnSave;
        public event Action OnCancel;

        public void Clean()
        {
            Entity = new Doctor();

        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;
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
                Doctor changedDoctor = (Doctor)fieldsGrid.DataContext;

                doctor.Id = changedDoctor.Id;
                doctor.Email = changedDoctor.Email;
                doctor.Name = changedDoctor.Name;
                doctor.PhoneNumber = changedDoctor.PhoneNumber;

                //It could be bound through the interface, but what I want to do is to allow that the errors are shown here
                ResultOperation result = docModel.Save(Entity);

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
        private bool ExecuteValidations()
        {
            bool returnVal = true;

            txtName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtEmail.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtPhoneNumber.GetBindingExpression(TextBox.TextProperty).UpdateSource();

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
        }
    }
}
