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
            string toAdmin = "false";

            try
            {
                email = e_mail.Text.ToString();
                password = pass.Text.ToString();
                sec_password = sec_pass.Text.ToString();
                if (czy_admin.IsChecked == true) toAdmin = "true";

                if (Check(email, password, sec_password))
                {
                    if (ServerComm.ServerCall("ca:" + email + ":" + password + ":" + toAdmin)[1] == "disapproved")
                        MessageBox.Show("Błąd, sprawdź dane i spróbuj poniwnie");
                    else
                        MessageBox.Show("Pomyślnie utworzono konto");
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

        private bool IsNotColon(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ':') return false;
            }
            return true;
        }

        private bool Check(string email, string pass1, string pass2)
        {
            if (email != "" && pass1 != "" && pass2 != "")
            {
                return (( IsNotColon(email) && IsNotColon(pass1) && IsNotColon(pass2) && CheckPasswords(pass1, pass2)));

            }
            else return false;
        }

        private bool CheckPasswords(string s1, string s2)
        {
            bool ok = false;
            for (int i = 0; i < s1.Length; i++)
            {
                if (Char.IsUpper(s1, i)) ok = true;
            }

            return ((s1 == s2) && ok && (s1.Length >= 8));
        }

        public LogOn()
        {
            InitializeComponent();
        }
    }
}
