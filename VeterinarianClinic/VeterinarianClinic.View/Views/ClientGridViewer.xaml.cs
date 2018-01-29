using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Interaction logic for ClientGridViewer.xaml
    /// </summary>
    public partial class ClientGridViewer : UserControl, IGridViewer<Client>
    {
        public ClientGridViewer()
        {
            InitializeComponent();
            Behavior = new BasicGridViewer<Client>(this);
        }

        private IModel<Client> clientModel = new ClientModel();
        public BasicGridViewer<Client> Behavior { get; set; }

        public BasicGrid GetBasicGrid()
        {
            return clientGrid;
        }

        public IEditor<Client> GetEditor()
        {
            return clientEditor;
        }

        public IModel<Client> GetModel()
        {
            return clientModel;
        }

        public Top GetTop()
        {
            return top;
        }

        public void ShowErrors(List<string> errors)
        {
            
        }

        public void Reload()
        {
            Behavior.Reload();
        }
    }
}
