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

namespace HangmanApp
{
    [Activity(Label = "OptionsActivity")]
    public class OptionsActivity : Activity
    {
        Database db;
        ImageView bckGround;
        TextView txt_OptionsWelcome;
        TextView txt_HighScore;
        TextView txt_Difficulty;
        RadioButton OptionsEasy;
        RadioButton OptionsNormal;
        RadioButton OptionsHard;
        int difficulty;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Options);
            db = new Database();
            difficulty = 2; //default selected radio button is normal which is 2
            txt_OptionsWelcome = FindViewById<TextView>(Resource.Id.txt_OptionsWelcome);
            txt_HighScore = FindViewById<TextView>(Resource.Id.txt_HighScore);
            txt_Difficulty = FindViewById<TextView>(Resource.Id.txt_OptionDifficulty);
            bckGround = FindViewById<ImageView>(Resource.Id.img_OptionsBackground);
            OptionsEasy = FindViewById<RadioButton>(Resource.Id.rb_OptionEasy);
            OptionsNormal = FindViewById<RadioButton>(Resource.Id.rb_OptionNormal);
            OptionsHard = FindViewById<RadioButton>(Resource.Id.rb_OptionHard);

            txt_HighScore.Text = "Your high score is: " + db.LoggedInAccount(this).HighestScore;
            bckGround.SetImageResource(Resource.Drawable.BackGround);
            OptionsEasy.Click += Rb_Difficulty_Click;
            OptionsNormal.Click += Rb_Difficulty_Click;
            OptionsHard.Click += Rb_Difficulty_Click;
            Helper.SetFonts(Assets, new List<View>()
                                    {
                                        txt_OptionsWelcome,
                                        txt_HighScore,
                                        txt_Difficulty,
                                        OptionsEasy,
                                        OptionsNormal,
                                        OptionsHard,
                                    });
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            db.Login(db.LoggedInAccount(this).Username, difficulty, true);
        }

        private void Rb_Difficulty_Click(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            switch (rb.Text)
            {
                case "Easy":
                    difficulty = 1;
                    break;
                case "Normal":
                    difficulty = 2;
                    break;
                case "Hard":
                    difficulty = 3;
                    break;
                default:
                    break;
            }
        }
    }
}