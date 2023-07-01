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
            string email = "";
            string pass = "";
            bool serverReply;
            try
            {
                email = e_mail.Text.ToString();
                pass = password.Text.ToString();

                if (email != "" && pass != "")
                {
                    //komunikacja z serwerem
                    serverReply = ServerComm.ServerCall("lc:"+email+":"+pass);
                    if(serverReply) this.NavigationService.Navigate(new UserPage());
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
                //zamknięcie połączenia
            }
        }

        public LogIn()
        {
            InitializeComponent();
        }
    }
}
