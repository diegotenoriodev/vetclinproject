using System.Windows;
using System.Windows.Controls;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// This control centralizes the menu items.
    /// It is also responsible for hidding/showing the options accordingly to the user's type.
    /// </summary>
    public partial class Menu : UserControl
    {
        /// <summary>
        /// Represents the available options
        /// </summary>
        public enum MenuOption
        {
            User,
            Clients,
            ClientsAddress,
            Pets,
            Doctors,
            Apointments
        }

        public delegate void MenuChanged(MenuOption option);

        /// <summary>
        /// This event is called when a user clicks on any button.
        /// Sending the MenuOption that was required.
        /// </summary>
        public event MenuChanged OnMenuChanged;

        public Menu()
        {
            InitializeComponent();
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.User);
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.Clients);
        }

        private void btnAddress_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.ClientsAddress);
        }

        private void btnPet_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.Pets);
        }

        private void btnDoctor_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.Doctors);
        }

        private void btnAppointment_Click(object sender, RoutedEventArgs e)
        {
            OnMenuChanged?.Invoke(MenuOption.Apointments);
        }

        /// <summary>
        /// Updates the shown options.
        /// </summary>
        public void Update()
        {
            if (!Login.UserLogin.IsAdmin)
            {
                backgroundRect.Height = 252 - btnUser.Height - 1;
                btnUser.Visibility = Visibility.Hidden;
            }
            else
            {
                btnUser.Visibility = Visibility.Visible;
                backgroundRect.Height = 252;
            }
        }
    }
}
