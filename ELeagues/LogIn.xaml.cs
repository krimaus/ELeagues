using Npgsql;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ELeagues
{
    /// <summary>
    /// Logika interakcji dla klasy LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public async void save(object sender, RoutedEventArgs e)
        {
            string user = "";
            string pass = "";
            bool serverReply;
            try
            {
                user = username.Text.ToString();
                pass = password.Text.ToString();

                if (Check(user, pass))
                {
                    //komunikacja z serwerem
                    serverReply = ServerComm.ServerCall("lc:"+ user + ":"+pass);
                    if (serverReply)
                    {
                        ServerComm.CurrentUser = user;
                        this.NavigationService.Navigate(new UserPage());
                    }
                    else MessageBox.Show("Błąd, sprawdź dane i spróbuj ponownie");
                }
                else
                {
                    MessageBox.Show("Nieprawidłowe dane, prosze wprowadź ponownie");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Coś poszło nie tak");
            }
        }

        public bool Check(string s1, string s2)
        {
            if (s1 != "" && s2 != "") return true;
            else return false;
        }

        public LogIn()
        {
            InitializeComponent();
        }
    }
}
