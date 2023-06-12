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
        public void save(object sender, RoutedEventArgs e)
        {
            string email = "";
            string password = "";
            string sec_password = "";
            var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString);

            try
            {
                conn.Open();

                email = e_mail.Text.ToString();
                password = pass.Text.ToString();
                sec_password = sec_pass.Text.ToString();

                string commandString = "SELECT max(id) FROM Uzyt;";
                using (var cmd = new NpgsqlCommand(commandString, conn)) 
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Trace.WriteLine(reader.GetString(0));
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coś poszło nie tak");
                conn.Close();
            }
        }

        public LogOn()
        {
            InitializeComponent();
        }
    }
}
