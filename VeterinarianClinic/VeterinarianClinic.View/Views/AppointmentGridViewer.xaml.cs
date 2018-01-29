using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Interaction logic for AppointmentGridViewer.xaml
    /// </summary>
    public partial class AppointmentGridViewer : UserControl, IGridViewer<Appointment>
    {
        private IModel<Appointment> appointmentModel = new AppointmentModel();
        public BasicGridViewer<Appointment> Behavior { get; set; }

        public AppointmentGridViewer()
        {
            InitializeComponent();
            Behavior = new BasicGridViewer<Appointment>(this);
        }

        public BasicGrid GetBasicGrid()
        {
            return appointmentGrid;
        }

        public IEditor<Appointment> GetEditor()
        {
            return appointmentEditor;
        }

        public IModel<Appointment> GetModel()
        {
            return appointmentModel;
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
            ///TODO
        }
    }
}
