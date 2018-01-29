using System;
using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Interaction logic for DoctorGridViewer.xaml
    /// </summary>
    public partial class DoctorGridViewer : UserControl, IGridViewer<Doctor>
    {
        private IModel<Doctor> docModel = new DoctorModel();
        public BasicGridViewer<Doctor> Behavior { get; set; }

        public DoctorGridViewer()
        {
            InitializeComponent();
            Behavior = new BasicGridViewer<Doctor>(this);
        }
        
        public BasicGrid GetBasicGrid()
        {
            return doctorGrid;
        }

        public IEditor<Doctor> GetEditor()
        {
            return doctorEditor;
        }

        public IModel<Doctor> GetModel()
        {
            return docModel;
        }

        public Top GetTop()
        {
            return top;
        }

        public void Reload()
        {
            Behavior.Reload();
        }

        public void ShowErrors(List<string> errors)
        {
        }
    }
}
