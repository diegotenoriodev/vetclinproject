using System.Windows.Controls;
using System.Windows.Media;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// This control presents the components for Title, New button and Search control.
    /// It is responsible for notifying through events when these components are used.
    /// It also shows messages of error and information about operations.
    /// </summary>
    public partial class Top : UserControl
    {
        public delegate void Click();

        /// <summary>
        /// It is called when the user clicks on New
        /// </summary>
        public event Click OnNewClick;

        /// <summary>
        /// It is fired when the users attempts to Search (through click or enter)
        /// </summary>
        public event Click OnSearchClick;

        /// <summary>
        /// Makes available the Content of search box
        /// </summary>
        public string SearchContent
        {
            get
            {
                return txtSearch.Text;
            }
            set
            {
                txtSearch.Text = value;
            }
        }

        /// <summary>
        /// Makes available the title
        /// </summary>
        public string Title
        {
            get
            {
                return lblTitle.Content.ToString();
            }
            set
            {
                lblTitle.Content = value;
            }
        }

        /// <summary>
        /// Makes available the tooltip for search box and button, therefore the developer
        /// will be able to display here what are the fields that the search take into account
        /// for each view.
        /// </summary>
        public string SearchToolTip
        {
            get
            {
                return txtSearch.ToolTip.ToString();
            }
            set
            {
                txtSearch.ToolTip = value;
                btnSearch.ToolTip = value;
            }
        }

        /// <summary>
        /// Developer can add specific information like "Click to add a new user"
        /// </summary>
        public string NewToolTip
        {
            get
            {
                return btnNew.ToolTip.ToString();
            }
            set
            {
                btnNew.ToolTip = value;
            }
        }

        public Top()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnNewClick?.Invoke();
        }

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnSearchClick?.Invoke();
        }

        private void txtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        public void AddError(string error)
        {
            lblMessage.Foreground = Brushes.Red;
            lblMessage.Content = error;
        }

        public void AddSuccess(string success = "Operation executed successfully")
        {
            lblMessage.Foreground = Brushes.ForestGreen;
            lblMessage.Content = success;
        }

        internal void ClearMessage()
        {
            lblMessage.Content = string.Empty;
        }
    }
}
