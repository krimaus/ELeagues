using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ELeagues
{
    /// <summary>
    /// Logika interakcji dla klasy LogOn.xaml
    /// </summary>
    public partial class LogOn : Page
    {
        public async void save(object sender, RoutedEventArgs e)
        {
            string email = "";
            string password = "";
            string sec_password = "";
            var conn = "--";
            //var dataSource = NpgsqlDataSource.Create(conn);
            //otwarcie połączenia z bazą

            try
            {
                email = e_mail.Text.ToString();
                password = pass.Text.ToString();
                sec_password = sec_pass.Text.ToString();

                if (password == sec_password && email != "" && sec_password != "")
                {
                    //zapytania do bazy

                    //zamknięcie połączenia z bazą
                }
                else
                {
                    MessageBox.Show("Nieprawidłowe dane, prosze wprowadź ponownie");
                }


                //zamknięcie połączenia z bazą
            }
            catch (Exception)
            {
                MessageBox.Show("Coś poszło nie tak");
                //conn.Close();
            }
        }

        public LogOn()
        {
            InitializeComponent();
        }
    }
}
