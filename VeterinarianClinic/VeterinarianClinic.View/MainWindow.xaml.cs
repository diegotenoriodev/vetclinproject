using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VeterinarianClinic.View.Views;

namespace VeterinarianClinic.View
{
    public partial class MainWindow : Window
    {
        private Dictionary<UserControls.Menu.MenuOption, UserControl> availableControls;
        public MainWindow()
        {
            InitializeComponent();
            menu.OnMenuChanged += Menu_OnMenuChanged;

            availableControls = new Dictionary<UserControls.Menu.MenuOption, UserControl>()
            {
                { UserControls.Menu.MenuOption.User, new Views.UserGridViewer() },
                { UserControls.Menu.MenuOption.Clients, new Views.ClientGridViewer() },
                { UserControls.Menu.MenuOption.ClientsAddress, new Views.AddressGridViewer() },
                { UserControls.Menu.MenuOption.Apointments, new Views.AppointmentGridViewer() },
                { UserControls.Menu.MenuOption.Pets, new Views.PetGridViewer() },
                { UserControls.Menu.MenuOption.Doctors, new Views.DoctorGridViewer() }
            };
        }

        private void Menu_OnMenuChanged(UserControls.Menu.MenuOption option)
        {
            if (availableControls.ContainsKey(option))
            {
                //We can remove this segment if we want to keep the state of our views
                if (availableControls[option] is IGridViewer)
                {
                    ((IGridViewer)availableControls[option]).Reload();
                }

                mainContent.Child = availableControls[option];
            }
        }

        private void loginPage_OnAuthenticated()
        {
            menu.Update();

            topMenu.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Visible;
            mainGrid.Visibility = Visibility.Visible;
            loginPage.Visibility = Visibility.Hidden;
            
            WindowState = WindowState.Maximized;

            lblUsername.Content = Login.UserLogin.Name;
        }

        private void exit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void aboutus_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("www.diegotenorio.com");
        }
    }
}
