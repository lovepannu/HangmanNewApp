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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        TextView txt_LoginWelcome;
        TextView txt_UserNamePrompt;
        TextView txt_DifficultyPrompt;
        EditText edt_Username;
        Button btn_Login;
        RadioButton rb_Easy;
        RadioButton rb_Normal;
        RadioButton rb_Hard;
        int difficulty = 2;
        ImageView bckGround;
        Database db;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            db = new Database();
            txt_LoginWelcome = FindViewById<TextView>(Resource.Id.txt_LoginWelcome);
            txt_UserNamePrompt = FindViewById<TextView>(Resource.Id.txt_UsernamePrompt);
            txt_DifficultyPrompt = FindViewById<TextView>(Resource.Id.txt_DifficultyPrompt);
            edt_Username = FindViewById<EditText>(Resource.Id.edt_Username);
            btn_Login = FindViewById<Button>(Resource.Id.btn_Login);
            rb_Easy = FindViewById<RadioButton>(Resource.Id.rb_Easy);
            rb_Normal = FindViewById<RadioButton>(Resource.Id.rb_Normal);
            rb_Hard = FindViewById<RadioButton>(Resource.Id.rb_Hard);
            bckGround = FindViewById<ImageView>(Resource.Id.img_LoginBackground);

            btn_Login.Click += Btn_Login_Click;
            rb_Easy.Click += Rb_Difficulty_Click;
            rb_Normal.Click += Rb_Difficulty_Click;
            rb_Hard.Click += Rb_Difficulty_Click;
            bckGround.SetImageResource(Resource.Drawable.BackGround);
            Helper.SetFonts(Assets, new List<View>()
                                    {
                                        txt_LoginWelcome,
                                        txt_DifficultyPrompt,
                                        txt_UserNamePrompt,
                                        edt_Username,
                                        btn_Login,
                                        rb_Easy,
                                        rb_Normal,
                                        rb_Hard
                                    });
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

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edt_Username.Text) || edt_Username.Text.Length < 1)
            {
                Helper.DisplayMessage(this, "Please enter a valid username");
                return;
            }
            db.Login(edt_Username.Text, difficulty);
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}