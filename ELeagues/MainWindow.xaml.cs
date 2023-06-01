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
        static byte rF = 69;
        static byte gF = 73;
        static byte bF = 80;

        static byte rB = 49;
        static byte gB = 53;
        static byte bB = 60;

        private Brush brushF = new SolidColorBrush(Color.FromRgb(rF, gF, bF));
        private Brush brushB = new SolidColorBrush(Color.FromRgb(rB, gB, bB));

        private void Focus1(object sender, RoutedEventArgs e)
        {
            btn1.Background = brushF;
        }

        private void Blur1(object sender, RoutedEventArgs e)
        {
            btn1.Background = brushB;
        }

        private void Focus2(object sender, RoutedEventArgs e)
        {
            btn2.Background = brushF;
        }

        private void Blur2(object sender, RoutedEventArgs e)
        {
            btn2.Background = brushB;
        }

        private void Focus3(object sender, RoutedEventArgs e)
        {
            btn3.Background = brushF;
        }

        private void Blur3(object sender, RoutedEventArgs e)
        {
            btn3.Background = brushB;
        }

        private void Focus4(object sender, RoutedEventArgs e)
        {
            btn4.Background = brushF;
        }

        private void Blur4(object sender, RoutedEventArgs e)
        {
            btn4.Background = brushB;
        }



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
