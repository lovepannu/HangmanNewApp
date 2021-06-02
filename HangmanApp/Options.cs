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
    public class Options
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public bool IsLoggedIn { get; set; }
        public int? LoggedInAccountID { get; set; }
        public int Difficulty { get; set; }

        public Options()
        {
            IsLoggedIn = false;
            LoggedInAccountID = null;
            Difficulty = 1; //1 = easy, 2 = normal, 3 = hard
        }
    }
}