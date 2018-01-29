using System.Collections.Generic;
using System.Windows.Controls;
using VeterinarianClinic.Domain;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// This is the implementation of a full grid viewer for user.
    /// The XAML presents three main controls:
    /// 1 - Top: that has the title and tooltips set,
    /// 2 - UserGrid with the required columns, and
    /// 3 - UserEditor
    /// </summary>
    public partial class UserGridViewer : UserControl, IGridViewer<User>
    {
        private IModel<User> userModel = new UserModel();
        public BasicGridViewer<User> Behavior { get; set; }

        public UserGridViewer()
        {
            InitializeComponent();
            
            //This line is where the bridge haapens and we can link this component
            //with the default behavior.
            //It is done here because this control cannot be child of another class even
            //if this class inherits from UserControl
            Behavior = new BasicGridViewer<User>(this);
        }

        //The only thing required for the grid to work here is the implementation of these methods:

        public BasicGrid GetBasicGrid()
        {
            return userGrid;
        }

        public IEditor<User> GetEditor()
        {
            return userEditor;
        }

        public IModel<User> GetModel()
        {
            return userModel;
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
