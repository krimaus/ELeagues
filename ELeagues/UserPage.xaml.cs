using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ELeagues
{
    /// <summary>
    /// Logika interakcji dla klasy UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public void Back(object sender, RoutedEventArgs e)
        {
            // zakładam że tutaj powinno być wylogowanie więc tu je wpisuje
            ServerComm.CurrentUser = null;
            this.NavigationService.Navigate(new NavigationPage());
        }

        public void CheckLogged(object sender, RoutedEventArgs e)
        {
            string helloMsg = "Witaj" + ServerComm.CurrentUser; // + \n +"Najblizsze turnieje na ktore jestes zapisany: " +info na jakie jest zapisane turnieje\n
            string turniejeAll = "erer"; //select na wszystkie turnieje
            hello_user.Content = helloMsg + "\n" + turniejeAll;

            //jezeli admin
            turnieje.Visibility = Visibility.Visible;
            dodajAdmina.Visibility = Visibility.Visible;

            //jezeli zwykly user
            turnieje.Visibility = Visibility.Hidden;
            dodajAdmina.Visibility = Visibility.Hidden;
        }

        public UserPage()
        {
            InitializeComponent();
        }
    }
}
