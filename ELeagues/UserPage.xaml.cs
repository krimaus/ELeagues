using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public int playersQuantity = 0;
        public List<string> usersToAdd = new List<string>();

        public void Back(object sender, RoutedEventArgs e)
        {
            // zakładam że tutaj powinno być wylogowanie więc tu je wpisuje
            ServerComm.CurrentUser = null;
            this.NavigationService.Navigate(new NavigationPage());
        }

        public void CheckLogged(object sender, RoutedEventArgs e)
        {
            string helloMsg = "Witaj" + ServerComm.CurrentUser; // + \n +"Najblizsze turnieje na ktore jestes zapisany: " +info na jakie jest zapisane turnieje\n
            string turniejeAll = ""; //select na wszystkie turnieje, postaram sie zrobic z tego przewijalna tabele
            hello_user.Content = helloMsg + "\n" + turniejeAll;

            //jezeli admin
            turnieje.Visibility = Visibility.Visible;
            dodajAdmina.Visibility = Visibility.Visible;

            //jezeli zwykly user
            turnieje.Visibility = Visibility.Hidden;
            dodajAdmina.Visibility = Visibility.Hidden;

            
        }

        public void AddPlayer(object sender, RoutedEventArgs e)
        {
            
            if (playersQuantity != 0)
            {
                string user = usernameGracza.ToString();
                usersToAdd.Add(user);
                playersQuantity--;
            }
            else
            {
                if (usersToAdd.Count != 0)
                {
                    save_button.Visibility = Visibility.Visible;
                }
            }
        }

        public void SaveTournament(object sender, RoutedEventArgs e)
        {
            //zapytania do bazy do tworzenia turnieju i przypisania do tego turnieju uzytkownikow z usersToAdd,
            //najlepiej z wykorzystaniem petli for each
        }

        public void CheckRounds(object sender, RoutedEventArgs e)
        {
            int rounds = 0;
            try
            {
                rounds = Int32.Parse(iloscRund.ToString());
                if (rounds % 2 == 0 && rounds != 0)
                {
                    playersQuantity = 2 * rounds;
                    //test.Content = "Ilosc graczy: " + playersQuantity.ToString(); - to tylko to moich testow
                }
                else
                {
                    MessageBox.Show("Ilosc rund powinna byc parzysta, spróbuj ponownie");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak");
            }
        }

        public UserPage()
        {
            InitializeComponent();
        }
    }
}
