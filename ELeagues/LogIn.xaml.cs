﻿using Npgsql;
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
            string[] serverAccept = { "sr", "approved" };
            string[] testCall;
            try
            {
                user = username.Text.ToString();
                pass = password.Text.ToString();

                if (Check(user, pass))
                {
                    testCall = ServerComm.ServerCall("sq:logincheck:" + user + ":" + pass);
                    //komunikacja z serwerem
                    if (testCall[0] == serverAccept[0] && testCall[1] == serverAccept[1])
                    {
                        ServerComm.CurrentUser = user;
                        if (ServerComm.ServerCall("sq:isadmin:" + user) == serverAccept) ServerComm.AdminStatus = true;
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

        public bool IsNotColon(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ':') return false;
            }
            return true;
        }

        public bool Check(string s1, string s2)
        {
            if (s1 != "" && s2 != "")
            {
                return ((IsNotColon(s1) && IsNotColon(s2)));
            }
            else return false;
        }

        public LogIn()
        {
            InitializeComponent();
        }
    }
}
