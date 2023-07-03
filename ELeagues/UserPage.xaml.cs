using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace ELeagues
{
    /// <summary>
    /// Logika interakcji dla klasy UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private int playersQuantity = 0;
        private int playersInList = 0;
        private List<string> usersToAdd = new List<string>();

        

        private void Back(object sender, RoutedEventArgs e)
        {
            // wylogowanie
            ServerComm.CurrentUser = null;
            ServerComm.AdminStatus = false;
            this.NavigationService.Navigate(new NavigationPage());
        }

        private void CheckLogged(object sender, RoutedEventArgs e)
        {
            ServerComm.AdminStatus = true;
            string helloMsg = "Witaj, " + ServerComm.CurrentUser + "\n" + "Turnieje do których jesteś zapisany/a:\n"; // + \n +"Najblizsze turnieje na ktore jestes zapisany: "
            // info na jakie jest zapisane turnieje\n
            foreach (string turneyName in ServerComm.ServerCall("sq:mytourneys:"+ServerComm.CurrentUser))
            {
                if (turneyName != "sr") helloMsg += turneyName + "\n";
            }

            string turniejeAll = ""; 
            //select na wszystkie turnieje, postaram sie zrobic z tego przewijalna tabele
            foreach(string turneyName in ServerComm.ServerCall("sq:alltourneys"))
            {
                if (turneyName != "sr") turniejeAll += turneyName + "\n";
            }
            
            hello_user.Content = helloMsg + "\n" + turniejeAll;

            
            if (ServerComm.AdminStatus)
            {
                //jezeli admin
                turnieje.Visibility = Visibility.Visible;
                editMatch.Visibility = Visibility.Visible;
            }
            else
            {
                //jezeli zwykly user
                turnieje.Visibility = Visibility.Hidden;
                editMatch.Visibility = Visibility.Hidden;
            }
        }

        private void AddPlayer(object sender, RoutedEventArgs e)
        {
            string user;
            try
            {
                if (playersQuantity > 0)
                {
                    user = usernameGracza.ToString();
                    usersToAdd.Add(user);
                    playersQuantity--;
                    usernameGracza.Text = "";
                    if (usersToAdd.Count == playersInList) save_button.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak");
            }
        }

        private void CreateContent(string query)
        {
            Window window = new Window(); //#FF31353C
            var bc = new BrushConverter();
            window.Background = (Brush)bc.ConvertFrom("#FF31353C");

            TextBlock text = new TextBlock();
            text.Text = query;
            text.Foreground = Brushes.White;
            text.FontSize = 18;
            text.Padding = new Thickness(20);

            Grid mainContent = new Grid();
            mainContent.Width = 400;
            mainContent.Margin = new Thickness(100, 0, 5, 0);
            mainContent.HorizontalAlignment = HorizontalAlignment.Center;
            mainContent.Height = 800;
            mainContent.Children.Add(text);

            window.Content = mainContent;
            window.Show();
        }

        private void ShowAllPlayers(object sender, RoutedEventArgs e)
        {
            string query = "";
             //select na wszystkich graczy
            CreateContent(query);
        }

        

        private void ShowMyTournaments(object sender, RoutedEventArgs e)
        {
            string query = ""; //select na turnieje danego gracza, tak zeby bylo widac w tym id ligi i id meczow, posortowane wzgledem id turnieju
            CreateContent(query);
        }

        private bool IsNumber(string s)
        {
            bool ok = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i])) ok = false;
            }
            return ok;
        }


        private void EditMatch(object sender, RoutedEventArgs e)
        {
            string g1 = gamer1.Text.ToString(), g2 = gamer2.Text.ToString();
            
            if (IsNumber(score1.Text.ToString()) && IsNumber(score2.Text.ToString()) && IsNumber((m_id.Text.ToString())))
            {
                int s1 = int.Parse(score1.Text.ToString()), s2 = int.Parse(score2.Text.ToString());
                int id = int.Parse(m_id.Text.ToString());

                if (g1 != "" && g2 != "" && g1 != g2)
                {
                    //zapytanie edytujace dany mecz o danym id
                    gamer1.Text = "";
                    gamer2.Text = "";
                    score1.Text = "";
                    score2.Text = "";
                }
            }
            
        }

        private void ShowEditButtons(object sender, RoutedEventArgs e)
        {
            lab_g1.Visibility = Visibility.Visible;
            lab_g2.Visibility = Visibility.Visible;
            lab_s1.Visibility = Visibility.Visible;
            lab_s2.Visibility = Visibility.Visible;

            score1.Visibility = Visibility.Visible;
            score2.Visibility = Visibility.Visible;
            gamer1.Visibility = Visibility.Visible;
            gamer2.Visibility = Visibility.Visible;

            edit.Visibility = Visibility.Visible;
        }

        private void ShowButtonM(object sender, RoutedEventArgs e)
        {
            edit_btns.Visibility = Visibility.Visible;
        }

        private void SaveTournament(object sender, RoutedEventArgs e)
        {
            //zapytania do bazy do tworzenia turnieju i przypisania do tego turnieju uzytkownikow z usersToAdd,
            //najlepiej z wykorzystaniem petli for each
            if (add_league.IsChecked == true)
            {
                //tworzy najpierw nowa lige, po tym tworzy turniej podpiety pod nowa lige
            }
            else
            {
                //tworzy turniej pod ostatnia lige
            }
        }

        private void CheckRounds(object sender, RoutedEventArgs e)
        {
            int rounds;
            //string r = "";
            try
            {
                rounds = int.Parse(iloscRund.Text.ToString());

                if (rounds % 2 == 0 && rounds != 0)
                {
                    playersQuantity = 2 * rounds;
                    playersInList = 2 * rounds;
                }
                else
                {
                    MessageBox.Show("Ilosc rund powinna byc parzysta, spróbuj ponownie");
                    iloscRund.Text = "2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak, rundy");
            }
        }

        public UserPage()
        {
            InitializeComponent();
        }
    }
}
