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
    /// Interaction logic for AppointmentEditor.xaml
    /// </summary>
    public partial class AppointmentEditor : UserControl, IEditor<Appointment>
    {
        private Appointment appointment;
        List<string> errors = new List<string>();
        private AppointmentModel appointmentModel;
        public Appointment Entity
        {
            get
            {
                return appointment;
            }
            set
            {
                appointment = value;
                if (appointment != null && appointment.Pet != null)
                {
                    appointment.Client = appointment.Pet.Owner;
                }
                appointment.Time = appointment.DateTimeOfAppointment.Hour;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = appointment.Clone();
                CleanComponents();
            }
        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;
        }

        public AppointmentEditor()
        {
            InitializeComponent();
            this.Entity = new Appointment();
            this.appointmentModel = new AppointmentModel();


            Validation.AddErrorHandler(cbxClient, ValidationError);
            Validation.AddErrorHandler(cbxPet, ValidationError);
            Validation.AddErrorHandler(cbxDoctor, ValidationError);
            Validation.AddErrorHandler(cbxAddress, ValidationError);
            Validation.AddErrorHandler(cbxAppointmentType, ValidationError);
            Validation.AddErrorHandler(dtpckrDate, ValidationError);
            Validation.AddErrorHandler(cbxTime, ValidationError);
        }

        public event Action OnSave;
        public event Action OnCancel;

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
                Appointment changedAppointment = (Appointment)fieldsGrid.DataContext;

                appointment.Address = changedAppointment.Address;
                appointment.AppointmentType = changedAppointment.AppointmentType;
                appointment.DateTimeOfAppointment = changedAppointment.DateTimeOfAppointment;
                appointment.Id = changedAppointment.Id;
                appointment.IdAddress = changedAppointment.IdAddress;
                appointment.IdAppointmentType = changedAppointment.IdAppointmentType;
                appointment.Client = changedAppointment.Client;
                appointment.Doctor = changedAppointment.Doctor;
                appointment.Pet = changedAppointment.Pet;

                //It could be bound through the interface, but what I want to do is to allow that the errors are shown here
                ResultOperation result = appointmentModel.Save(changedAppointment);

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

            cbxClient.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            cbxPet.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            cbxDoctor.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            cbxAppointmentType.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            cbxAddress.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            dtpckrDate.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
            cbxTime.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

            if (errors.Any()) { returnVal = false; }

            return returnVal;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Clean();
            OnCancel?.Invoke();
        }

        public void Clean()
        {
            Entity = new Appointment();
            cbxTime.Text = "";
        }

        public void Reload()
        {
            AppointmentControl_Loaded(null, null);
        }

        public Appointment ChangedAppointment { get => (Appointment)fieldsGrid.DataContext; }

        private void AppointmentControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbxClient.ItemsSource = appointmentModel.GetClients();
            if (ChangedAppointment.Client != null)
            {
                this.cbxPet.ItemsSource = appointmentModel.GetPets(ChangedAppointment.Client.Id);
                this.cbxAddress.ItemsSource = appointmentModel.GetAddresses(ChangedAppointment.Client.Id);
            }
            this.cbxDoctor.ItemsSource = appointmentModel.GetDoctors();
            this.cbxAppointmentType.ItemsSource = appointmentModel.GetAppointmentTypes();
            this.dtpckrDate.DisplayDateStart = DateTime.Now;
            if (ChangedAppointment.Doctor != null && ChangedAppointment.DateTimeOfAppointment != null)
            {
                this.cbxTime.ItemsSource = this.appointmentModel.GetAvailableHours(ChangedAppointment.Id, ChangedAppointment.Doctor, ChangedAppointment.DateTimeOfAppointment);
            }
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

        private void ClientValueChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChangedAppointment.Client != null)
            {
                this.cbxPet.ItemsSource = appointmentModel.GetPets(ChangedAppointment.Client.Id);
                this.cbxAddress.ItemsSource = appointmentModel.GetAddresses(ChangedAppointment.Client.Id);
            }
        }

        private void UpdateTimeItemSource(object sender, SelectionChangedEventArgs e)
        {
            if (ChangedAppointment.Doctor != null && ChangedAppointment.DateTimeOfAppointment != null)
            {
                this.cbxTime.ItemsSource = this.appointmentModel.GetAvailableHours(ChangedAppointment.Id, ChangedAppointment.Doctor, ChangedAppointment.DateTimeOfAppointment);
            }
        }
    }
}
