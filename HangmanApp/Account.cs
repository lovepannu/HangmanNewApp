using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace HangmanApp
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Username { get; set; }
        public int HighestScore { get; set; }
        public int AccountOptionsID { get; set; }
        [Ignore]
        public Options AccountOptions { get; set; }

        public Account()
        {
            Username = "Player1";
            HighestScore = 0;
            AccountOptions = new Options();
            AccountOptions.IsLoggedIn = true;
            AccountOptions.LoggedInAccountID = ID;
        }
    }
}