using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Interaction logic for PetGridViewer.xaml
    /// </summary>
    public partial class PetGridViewer : UserControl, IGridViewer<Pet>
    {
        private IModel<Pet> petModel = new PetModel();
        public BasicGridViewer<Pet> Behavior { get; set; }

        public PetGridViewer()
        {
            InitializeComponent();
            Behavior = new BasicGridViewer<Pet>(this);
        }
        
        public BasicGrid GetBasicGrid()
        {
            return petGrid;
        }
        
        public IModel<Pet> GetModel()
        {
            return petModel;
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

        public IEditor<Pet> GetEditor()
        {
            return petEditor;
        }
    }
}
