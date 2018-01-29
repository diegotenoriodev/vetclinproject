using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using VeterinarianClinic.Model;
using VeterinarianClinic.View.UserControls;

namespace VeterinarianClinic.View.Views
{
    /// <summary>
    /// Base interface for GridViewer
    /// GridViewer are classes that put together all components for executing a CRUD.
    /// </summary>
    public interface IGridViewer
    {
        /// <summary>
        /// Returns the datacontext from the control
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// A viewer should have a BasicGrid
        /// </summary>
        BasicGrid GetBasicGrid();

        /// <summary>
        /// A viewer should have a Top
        /// </summary>
        Top GetTop();

        /// <summary>
        /// Gives the view the possibility for dealing with errors.
        /// </summary>
        void ShowErrors(List<string> errors);

        /// <summary>
        /// A viewer should have a function for reloading the lists and components
        /// </summary>
        void Reload();
    }

    /// <summary>
    /// Generic interface for Grid Viewer
    /// </summary>
    /// <typeparam name="T">Represents a entity from the domain</typeparam>
    public interface IGridViewer<T> : IGridViewer
    {
        /// <summary>
        /// A view should present a behavior of T.
        /// This is how the behaviors are authomated.
        /// </summary>
        BasicGridViewer<T> Behavior { get; set; }

        /// <summary>
        /// It should have a model for the domain entity
        /// </summary>
        Model.IModel<T> GetModel();

        /// <summary>
        /// It should have an editor for the domain entity
        /// </summary>
        /// <returns></returns>
        IEditor<T> GetEditor();
    }

    /// <summary>
    /// This is the brain for grid viewer. The proble was, we could not change the inheritance
    /// of a xaml class because every time we changed something in the xaml, the class would change
    /// the inheritance again to UserControl. So, this was the way we found. This is a application
    /// of the Bridge pattern, this class will be responsible for changing the behavior of the
    /// grid view to make things work automaticly, avoiding rework and centralizing the code.
    /// Basicaly it presents a Observable collection and sets it to the Grid datasource.
    /// It also has the function of setting default listeners to events from the following components:
    /// Top, BasicGrid and IEditor.
    /// It links their events to IModel methods, so the developer will not have to write code
    /// for Delete or Edit, and fill the ObservableCollection with data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicGridViewer<T> : UserControl
    {
        /// <summary>
        /// Usage of observable.
        /// The only place where the list is bond is the code GetBasicGrid().ItemsSource = List
        /// in the constructor. Therefore, WPF is responsible for updating the grid at all events.
        /// It is even better seen when we remove an item from the list.
        /// </summary>
        protected ObservableCollection<T> List { get; set; }

        /// <summary>
        /// It is the gridviewer that is adopting this behavior.
        /// </summary>
        protected IGridViewer<T> GridImplementation;

        /// <summary>
        /// The specific model from the implementation
        /// </summary>
        private Model.IModel<T> GetModel() { return GridImplementation.GetModel(); }

        /// <summary>
        /// The specific editor from the implementation
        /// </summary>
        private IEditor<T> GetEditor() { return GridImplementation.GetEditor(); }

        /// <summary>
        /// The specific grid from the implementation
        /// </summary>
        private BasicGrid GetBasicGrid() { return GridImplementation.GetBasicGrid(); }

        /// <summary>
        /// The specific top from the implementation
        /// </summary>
        private Top GetTop() { return GridImplementation.GetTop(); }

        /// <summary>
        /// Sending errors to the implementation
        /// </summary>
        private void ShowErrors(List<string> errors) { GridImplementation.ShowErrors(errors); }

        /// <summary>
        /// Constructor should receive the GridViewer that is adopting this behavior
        /// </summary>
        public BasicGridViewer(IGridViewer<T> gridViewer)
        {
            GridImplementation = gridViewer;

            //This is set as the datacontext for the GridViewer
            GridImplementation.DataContext = this;

            //The observable collection is instantiated with all items for the model
            List = new ObservableCollection<T>(GetModel().GetAll(string.Empty));

            //The list is set, and this is the only place where it is done.
            GetBasicGrid().ItemsSource = List;

            //Listeners for OnEdit and OnDelete with pre-defined behavior
            GetBasicGrid().OnEdit += BaseGridViewer_OnEdit;
            GetBasicGrid().OnDelete += BaseGridViewer_OnDelete;

            //Listeners for OnCancel and OnSave with pre-defined behavior
            GetEditor().OnCancel += BaseGridViewer_OnCancel;
            GetEditor().OnSave += BaseGridViewer_OnSave;

            //OnNew and OnSearch have also pre-defined behavior
            GetTop().OnNewClick += BaseGridViewer_OnNewClick;
            GetTop().OnSearchClick += BaseGridViewer_OnSearchClick;

            //This pre-defined behavior is a way of making the framework do more and
            //reduce the amount of code written by developers.
        }

        /// <summary>
        /// Hides the editor, Calls reload for the editor
        /// </summary>
        public void Reload()
        {
            HideEditor();
            GetEditor().Reload();
            BaseGridViewer_OnSearchClick();
            GetTop().ClearMessage();
        }

        /// <summary>
        /// When the onSearch is called, the GetAll method from Model of T is called,
        /// sending the Content informed on the search box. The model is responsible for
        /// get the data and execute the filters, then it retrieves it, and sets to the
        /// Observable collection. Doing so, there is no need to rebind the grid with the 
        /// collection, the Observable does it.
        /// </summary>
        private void BaseGridViewer_OnSearchClick()
        {
            var newList = GetModel().GetAll(GetTop().SearchContent);
            List.Clear();

            foreach (var item in newList)
            {
                List.Add(item);
            }
        }

        /// <summary>
        /// The pre-defined behavior for click on the button new is to clean the content from
        /// the editor and show it, hiding the top and grid.
        /// </summary>
        private void BaseGridViewer_OnNewClick()
        {
            GetEditor().Clean();
            ShowEditor();
        }

        /// <summary>
        /// OnSave is called only if everything goes Okay.
        /// It verifies if the entity exists in the list (it it does not, it means that it was 
        /// an inclusion of a new item), and it adds to the ObservableCollection that automaticly
        /// changes the items shown in the grid.
        /// It also calls AddSuccess from the Top component and hides the editor (and shows the grid and top).
        /// </summary>
        private void BaseGridViewer_OnSave()
        {
            if (List.Contains(GetEditor().Entity))
            {
                List.Remove(GetEditor().Entity);
            }

            //Inserting in order to show the item in the first position in the grid
            //therefore the user would know easily the item that he added or changed
            List.Insert(0, GetEditor().Entity);

            GetTop().AddSuccess();

            HideEditor();
        }

        /// <summary>
        /// When a user cancels the edition or inclution, we just need to hide the editor (showing the grid and top)
        /// and clear any message from the top.
        /// </summary>
        private void BaseGridViewer_OnCancel()
        {
            HideEditor();
            GetTop().ClearMessage();
        }

        /// <summary>
        /// The pre-defined behavior is call the Delete method from the Model.
        /// This method is responsible for verifying the possibility of removal. It returns
        /// a ResultOperation that can be successiful or not, giving the reason for that through
        /// the Errors property.
        /// If the operation is executed, it adds to the top the success message, otherwise,
        /// it shows the errors returned by the model.
        /// </summary>
        /// <param name="item"></param>
        private void BaseGridViewer_OnDelete(object item)
        {
            GetTop().ClearMessage();
            ResultOperation result = GetModel().Delete((T)item);

            if (result.Success)
            {
                List.Remove((T)item);
                GetTop().AddSuccess();
            }
            else
            {
                ShowErrors(result.Errors);
                GetTop().AddError(UIHelper.GetStringFromList(result.Errors));
            }
        }

        /// <summary>
        /// Sets the edited item to the editor and opens it.
        /// </summary>
        /// <param name="item"></param>
        private void BaseGridViewer_OnEdit(object item)
        {
            GetTop().ClearMessage();
            GetEditor().Entity = (T)item;
            ShowEditor();
        }

        protected virtual void ShowEditor()
        {
            GetEditor().Visibility = System.Windows.Visibility.Visible;
            GetTop().IsEnabled = false;
            GetBasicGrid().IsEnabled = false;
        }

        protected virtual void HideEditor()
        {
            GetEditor().Visibility = System.Windows.Visibility.Hidden;
            GetTop().IsEnabled = true;
            GetBasicGrid().IsEnabled = true;
        }
    }
}
