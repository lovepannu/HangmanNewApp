using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HangmanApp
{
    [Table ("tbl_Words")]
    public class Word
    {
        [PrimaryKey,Unique, AutoIncrement, Column("id"), NotNull]
        public int id { get; set; }

        [MaxLength(10),NotNull, Column("word")]
        public string word { get; set; }
    }
}