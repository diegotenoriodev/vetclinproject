using System;
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
    /// Interaction logic for PetEditor.xaml
    /// </summary>
    public partial class PetEditor : UserControl, IEditor<Pet>
    {

        private Pet pet;
        private DateTime date = DateTime.Now;
        public Pet Entity
        {
            get
            {
                return pet;
            }
            set
            {
                CleanComponents();
                pet = value;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = pet.Clone();
            }
        }

        private PetModel petModel;

        public PetModel PetModel { get => petModel; set => petModel = value; }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public PetEditor()
        {
            InitializeComponent();

            Entity = new Pet();
            petModel = new PetModel();

            //An animal can not be born in the future
            datePicker.DisplayDateEnd = DateTime.Now.Date;

            Loaded += PetEditor_Loaded;

            Validation.AddErrorHandler(txtName, ValidationError);
            Validation.AddErrorHandler(cbxSpecie, ValidationError);
            Validation.AddErrorHandler(txtWeight, ValidationError);
            Validation.AddErrorHandler(datePicker, ValidationError);
            Validation.AddErrorHandler(cbxOwner, ValidationError);
        }

        public event Action OnSave;
        public event Action OnCancel;

        List<string> errors = new List<string>();

        public void Clean()
        {
            Entity = new Pet();
            CleanComponents();
        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;

        }
        public void Reload()
        {
            PetEditor_Loaded(null, null);
        }
        private void PetEditor_Loaded(object sender, RoutedEventArgs e)
        {
            cbxOwner.ItemsSource = petModel.GetClients(string.Empty);
            cbxSpecie.ItemsSource = petModel.GetSpecies();
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
        private bool ExecuteValidations()
        {

            bool returnVal = true;
            txtName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            cbxSpecie.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            txtWeight.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            datePicker.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            cbxOwner.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            if (errors.Any()) { returnVal = false; }

            return returnVal;
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
                Pet petChanged = (Pet)fieldsGrid.DataContext;
                
                Entity.BirthDay = petChanged.BirthDay;
                Entity.Id = petChanged.Id;
                Entity.IdOwner = petChanged.IdOwner;
                Entity.IdSpecie = petChanged.IdSpecie;
                Entity.Name = petChanged.Name;
                Entity.Owner = petChanged.Owner;
                Entity.Race = petChanged.Race;
                Entity.Specie = petChanged.Specie;
                Entity.Weight = petChanged.Weight;

                ResultOperation result = petModel.Save(Entity);

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

        private void AddError(string str)
        {
            errors.Add(str);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Clean();
            OnCancel?.Invoke();
        }

    }

}
