using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string helloMsg = "Witaj, " + ServerComm.CurrentUser + "\n" + "Turnieje do których jesteś zapisany/a:\n"; // + \n +"Najblizsze turnieje na ktore jestes zapisany: "
            // info na jakie jest zapisane turnieje 
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
            var allPlayers = ServerComm.ServerCall("sq:allplayers");
            foreach(var player in allPlayers)
            {
                query += player + "\n";
            }
            CreateContent(query);
        }

        private void ShowMyTournaments(object sender, RoutedEventArgs e)
        {
            string query = ""; 
            //select na turnieje danego gracza, tak zeby bylo widac w tym id ligi i id meczow, posortowane wzgledem id turnieju
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

        private bool IsPowerOfTwo_2(int number)
        {
            double log = Math.Log(number, 2);
            double pow = Math.Pow(2, Math.Round(log));
            return pow == number;
        }

        private void EditMatch(object sender, RoutedEventArgs e)
        {
            string g1 = gamer1.Text.ToString(), g2 = gamer2.Text.ToString();
            
            if (IsNumber(score1.Text.ToString()) && IsNumber(score2.Text.ToString()) && IsNumber((m_id.Text.ToString())))
            {
                string s1 = score1.Text.ToString(), s2 = score2.Text.ToString();
                string id = m_id.Text.ToString();
                if (g1 == "") g1 = "empty";
                if (g2 == "") g2 = "empty";
                if (s1 == "") s1 = "empty";
                if (s2 == "") s2 = "empty";


                if (g1 != "" && g2 != "" && g1 != g2)
                {
                    //zapytanie edytujace dany mecz o danym id
                    ServerComm.ServerCall("em:" + id.ToString() + ":" + g1 + ":" + g2 + ":" + s1 + ":" + s2 + ":empty");
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
            int i,j;
            string idHolder, matchIdHolder;
            List<string> currentRoundMatchIds = new(), nextRoundMatchIds = new();

            if (add_league.IsChecked == true)
            {
                //tworzy najpierw nowa lige, po tym tworzy turniej podpiety pod nowa lige
                idHolder = ServerComm.ServerCall("cl:" + ServerComm.CurrentUser)[1];
                idHolder = ServerComm.ServerCall("ct:" + idHolder)[1];
            }
            else
            {
                //tworzy turniej pod ostatnia lige chyba że taka nie istnieje
                var leagueReply = ServerComm.ServerCall("sq:lastusedleague:" + ServerComm.CurrentUser);
                if (!leagueReply[1].Equals("disapproved"))
                {
                    idHolder = ServerComm.ServerCall("ct:" + leagueReply[1])[1];
                }
                else
                {
                    idHolder = ServerComm.ServerCall("cl:" + ServerComm.CurrentUser)[1];
                    idHolder = ServerComm.ServerCall("ct:" + idHolder)[1];
                }
                
            }

            // petla tworząca mecze pierwszej rundy
            for(i = 0; i<usersToAdd.Count()/2; i++)
            {
                currentRoundMatchIds.Add(ServerComm.ServerCall("cm:" + idHolder)[1]);
            }

            // pętla wypełniająca pierwszą rundę
            for(i = 0; i<currentRoundMatchIds.Count(); i++)
            {
                ServerComm.ServerCall("em:" + currentRoundMatchIds[i] + ":" + idHolder + ":" + usersToAdd[i] + ":" + usersToAdd[2*i] + ":empty:empty:empty");
            }

            // pętla dopisująca resztę meczy i łącząca kolejne rundy
            while(currentRoundMatchIds.Count() != 1)
            {
                for(j = 0; j<currentRoundMatchIds.Count()/2; j++)
                {
                    matchIdHolder = ServerComm.ServerCall("cm:" + idHolder)[1];
                    ServerComm.ServerCall("em:" + currentRoundMatchIds[j] + ":empty:empty:empty:empty:" + matchIdHolder);
                    ServerComm.ServerCall("em:" + currentRoundMatchIds[currentRoundMatchIds.Count() - j] + ":empty:empty:empty:empty:" + matchIdHolder);
                    nextRoundMatchIds.Add(matchIdHolder);
                }
                currentRoundMatchIds.Clear();
                currentRoundMatchIds = nextRoundMatchIds;
            }

            // finał
            // matchIdHolder = ServerComm.ServerCall("cm:" + idHolder)[1];
            // ServerComm.ServerCall("em:" + currentRoundMatchIds[0] + ":empty:empty:empty:empty:" + matchIdHolder);
            // ServerComm.ServerCall("em:" + currentRoundMatchIds[1] + ":empty:empty:empty:empty:" + matchIdHolder);

        }

        private void CheckRounds(object sender, RoutedEventArgs e)
        {
            int rounds;
            //string r = "";
            try
            {
                rounds = int.Parse(iloscRund.Text.ToString());

                if (IsPowerOfTwo_2(rounds))
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
