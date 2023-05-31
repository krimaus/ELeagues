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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT version();", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Trace.WriteLine(reader.GetString(0));
                }
            }

            //zamiast bawic sie z licznikami w sql robic to po stronie aplikacji???
            //wykrywac pierwsze nieuzyte id i przypisywac
            /*
            string id = "5";
            string username = "monalisaEnjoyer";
            string table = "eleagues";
            string commandText = "INSERT INTO " + table + " VALUES (\'" + username + "\', " + id + ");";

            using (var cmd = new NpgsqlCommand(commandText, conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    Trace.WriteLine("FUBAR sql command");
                    Close();
                }
            }
            */
            var dataSource = NpgsqlDataSource.Create(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString);

            using (var cmd = dataSource.CreateCommand("SELECT * FROM eleagues"))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }

            InitializeComponent();
        }
    }
}
