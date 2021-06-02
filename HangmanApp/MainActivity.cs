using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using SQLite;
using Android.Graphics;
using Android.Views;
using System.Collections.Generic;

namespace HangmanApp
{
    [Activity(Label = "HangmanApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView txt_Welcome;
        Button btn_Start;
        Button btn_LogIn;
        Button btn_Account;
        ImageView bckGround;
        //Database db;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
           // db = new Database();
            txt_Welcome = FindViewById<TextView>(Resource.Id.txt_Welcome);
            btn_Start = FindViewById<Button>(Resource.Id.btn_Start);
            btn_LogIn = FindViewById<Button>(Resource.Id.btn_MainLogIn);
            btn_Account = FindViewById<Button>(Resource.Id.btn_Account);
            bckGround = FindViewById<ImageView>(Resource.Id.img_Splash);
            
           /* if (db.IsLoggedIn())
            {*/
              //  txt_Welcome.Text = "Welcome " + db.LoggedInAccount(this).Username + "\n To The HangMan Game";
                btn_LogIn.Visibility = ViewStates.Visible;
                btn_Account.Visibility = ViewStates.Visible;
            //}
            Helper.SetFonts(Assets, new List<View>()
                                    {
                                        txt_Welcome,
                                        btn_Start,
                                        btn_LogIn,
                                        btn_Account
                                    });
            bckGround.SetImageResource(Resource.Drawable.BackGround);
            btn_Start.Click += Btn_Start_Click;
            btn_LogIn.Click += Btn_LogOut_Click;
            btn_Account.Click += Btn_Account_Click; ;
        }

        private void Btn_Account_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(OptionsActivity));//no finish cause we might want to go back to start screen
        }

        private void Btn_LogOut_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
            Finish();
        }

        private void Btn_Start_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(GameActivity));
            /*  if (db.IsLoggedIn())
              {

                  Finish();
              }
              else
              {
                  StartActivity(typeof(LoginActivity));
                  Finish();
              }*/

        }
    }
}

