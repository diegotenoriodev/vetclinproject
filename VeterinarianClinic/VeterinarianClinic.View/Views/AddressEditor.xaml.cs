﻿using System;
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
    /// Interaction logic for AddressEditor.xaml
    /// </summary>
    public partial class AddressEditor : UserControl, IEditor<Address>
    {
        public IList<Province> Provinces
        {
            get
            {
                return Enum.GetValues(typeof(Province)).Cast<Province>().ToList<Province>();
            }
        }

        private Address address;
        List<string> errors = new List<string>();
        private AddressModel addressModel;

        public AddressEditor()
        {
            InitializeComponent();
            this.Entity = new Address();
            this.addressModel = new AddressModel();
            cbxProvince.ItemsSource = Provinces;

            Validation.AddErrorHandler(cbxClient, ValidationError);
            Validation.AddErrorHandler(txtLine1, ValidationError);
            Validation.AddErrorHandler(txtApartment, ValidationError);
            Validation.AddErrorHandler(txtCity, ValidationError);
            Validation.AddErrorHandler(cbxProvince, ValidationError);
            Validation.AddErrorHandler(txtPostalCode, ValidationError);
        }

        public Address Entity
        {
            get
            {
                return address;
            }
            set
            {
                CleanComponents();
                address = value;

                //We give a clone of the entity for the WPF to work with
                fieldsGrid.DataContext = address.Clone();
            }
        }

        private void CleanComponents()
        {
            errors.Clear();
            lblErrors.Content = string.Empty;
        }

        public event Action OnSave;
        public event Action OnCancel;

        public void Clean()
        {
            Entity = new Address();
        }

        public void Reload()
        {
            AddressControl_Loaded(null, null);
        }

        private void AddressControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbxClient.ItemsSource = addressModel.GetClients();
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
                Address addressEdit = (Address)fieldsGrid.DataContext;

                address.Id = addressEdit.Id;
                address.Line1 = addressEdit.Line1;
                address.Apartment = addressEdit.Apartment;
                address.City = addressEdit.City;
                address.Province = addressEdit.Province;
                address.PostalCode = addressEdit.PostalCode;
                address.Client = addressEdit.Client;

                //It could be bound through the interface, but what I want to do is to allow that the errors are shown here
                ResultOperation result = addressModel.Save(Entity);

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
            txtLine1.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtApartment.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            txtCity.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            cbxProvince.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            txtPostalCode.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (errors.Any()) { returnVal = false; }

            return returnVal;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Clean();
            OnCancel?.Invoke();
        }

    }
}
