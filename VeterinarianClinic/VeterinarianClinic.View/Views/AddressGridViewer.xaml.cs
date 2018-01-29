using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Interaction logic for AddressGridViewer.xaml
    /// </summary>
    public partial class AddressGridViewer : UserControl, IGridViewer<Address>
    {
        private IModel<Address> addressModel = new AddressModel();
        public BasicGridViewer<Address> Behavior { get; set; }

        public AddressGridViewer()
        {
            InitializeComponent();
            Behavior = new BasicGridViewer<Address>(this);
        }

        public BasicGrid GetBasicGrid()
        {
            return addressGrid;
        }

        public IEditor<Address> GetEditor()
        {
            return addressEditor;
        }

        public IModel<Address> GetModel()
        {
            return addressModel;
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
