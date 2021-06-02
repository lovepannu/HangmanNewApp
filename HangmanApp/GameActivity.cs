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
using Android.Graphics.Drawables;
using Android.Views.Animations;
using System.Timers;

namespace HangmanApp
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        class timerState
        {
            public int Counter { get; set; }
            public Timer animTimer;
            public timerState()
            {
                Counter = 0;
            }
        }
        timerState ts = new timerState();
        Database db = new Database();
        TextView txt_WordDisplay;
        TextView txt_ScoreDisplay;
        TextView txt_ChanceDisplay;
        ImageView img_Hang;
        List<int> HangingImageResources;
        Button btn_NewGame;
        Button btn_Quit;
        LinearLayout qRow;
        LinearLayout aRow;
        LinearLayout zRow;
        List<Button> QwertyList;
        ImageView bckGround;
        string WordToGuess;
        int difficulty;
        int chancesLeft;
        int score;
        bool Restore = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Game);

            //misc
            difficulty = db.CurrentOptions(this).Difficulty;
            score = 0;
            //database and words
            string dbPath = Helper.GetLocalFilePath("words.db3");
            Database db_Words = new Database(dbPath);
            WordToGuess = db_Words.GetRandomWord(difficulty);
            QwertyList = new List<Button>();
            //controls
            txt_WordDisplay = FindViewById<TextView>(Resource.Id.txt_WordDisplay);
            txt_ScoreDisplay = FindViewById<TextView>(Resource.Id.txt_ScoreDisplay);
            txt_ChanceDisplay = FindViewById<TextView>(Resource.Id.txt_ChanceDisplay);
            btn_NewGame = FindViewById<Button>(Resource.Id.btn_NewGame);
            btn_Quit = FindViewById<Button>(Resource.Id.btn_Quit);
            qRow = FindViewById<LinearLayout>(Resource.Id.ll_Qrow);
            aRow = FindViewById<LinearLayout>(Resource.Id.ll_Arow);
            zRow = FindViewById<LinearLayout>(Resource.Id.ll_Zrow);
            bckGround = FindViewById<ImageView>(Resource.Id.img_GameBackground);
            img_Hang = FindViewById<ImageView>(Resource.Id.img_Hanging);
            //timer
            ts.animTimer = new Timer(50); //50 means 20 frames per second 33 means 30 frames per second

            //State Management
            if (savedInstanceState != null)
            {
                difficulty = savedInstanceState.GetInt("difficulty", difficulty);
                score = savedInstanceState.GetInt("score", score);
                WordToGuess = savedInstanceState.GetString("WordToGuess", WordToGuess);
                if (savedInstanceState.GetBooleanArray("buttonEnabledArray") != null)
                {
                    Restore = true;
                }
            }

            //Toast.MakeText(this, "Difficulty: " + db.CurrentOptions(this).Difficulty, ToastLength.Long).Show();
            //Helper.DisplayMessage(this, WordToGuess, ToastLength.Long);

            if (Restore)
                txt_WordDisplay.Text = savedInstanceState.GetString("WordDisplay", string.Join("", Enumerable.Repeat("_ ", WordToGuess.Length).ToArray()));
            else
                txt_WordDisplay.Text = string.Join("", Enumerable.Repeat("_ ", WordToGuess.Length).ToArray());
            HangingImageResources = new List<int>();
            switch (difficulty)
            {
                case 1://easy
                    HangingImageResources.Add(Resource.Drawable.Hang1); HangingImageResources.Add(Resource.Drawable.Hang2);
                    HangingImageResources.Add(Resource.Drawable.Hang3); HangingImageResources.Add(Resource.Drawable.Hang4);
                    HangingImageResources.Add(Resource.Drawable.Hang5); HangingImageResources.Add(Resource.Drawable.Hang6);
                    HangingImageResources.Add(Resource.Drawable.Hang7); HangingImageResources.Add(Resource.Drawable.Hang8);
                    HangingImageResources.Add(Resource.Drawable.Hang9);
                    break;
                case 2://normal
                    HangingImageResources.Add(Resource.Drawable.Hang2); HangingImageResources.Add(Resource.Drawable.Hang4);
                    HangingImageResources.Add(Resource.Drawable.Hang6); HangingImageResources.Add(Resource.Drawable.Hang8);
                    HangingImageResources.Add(Resource.Drawable.Hang9);
                    break;
                case 3://hard
                    HangingImageResources.Add(Resource.Drawable.Hang3);
                    HangingImageResources.Add(Resource.Drawable.Hang5);
                    HangingImageResources.Add(Resource.Drawable.Hang7);
                    HangingImageResources.Add(Resource.Drawable.Hang9);
                    break;
                default:
                    break;
            }
            if (savedInstanceState != null)
                chancesLeft = savedInstanceState.GetInt("chancesLeft", chancesLeft);
            else
                chancesLeft = HangingImageResources.Count();
            txt_ChanceDisplay.Text = "Chances left: " + chancesLeft;
            txt_ScoreDisplay.Text = "Score: " + score;
            bckGround.SetBackgroundResource(Resource.Drawable.BackGround);
            img_Hang.SetImageResource(Resource.Drawable.blank);
            btn_NewGame.Click += Btn_NewGame_Click;
            btn_Quit.Click += Btn_Quit_Click;

            //setup 3 rows filled with the letters
            foreach (LinearLayout item in new List<LinearLayout>() { qRow, aRow, zRow })
            {
                string rowOfCharacters = "";
                if (item == qRow) rowOfCharacters = "qwertyuiop";
                if (item == aRow) rowOfCharacters = "asdfghjkl";
                if (item == zRow) rowOfCharacters = "zxcvbnm";

                foreach (char c in rowOfCharacters)
                {
                    Button btn_newButton = new Button(this);
                    btn_newButton.Text = c.ToString().ToUpper();
                    LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    param.SetMargins(1, 1, 0, 0);
                    item.AddView(btn_newButton, param);
                    btn_newButton.Click += LetterClicked;
                    QwertyList.Add(btn_newButton);
                }
                item.SetGravity(GravityFlags.CenterHorizontal);
            }
            if (Restore)
            {
                var boolA = savedInstanceState.GetBooleanArray("buttonEnabledArray");
                for (int i = 0; i < boolA.Length; i++)
                {
                    QwertyList[i].Enabled = boolA[i];
                }
                img_Hang.SetImageResource(HangingImageResources[Math.Abs(chancesLeft - HangingImageResources.Count) - 1]);
            }
            Helper.SetFonts(Assets, new List<View>() //set the fonts of the controls
                                    {
                                        txt_WordDisplay,
                                        txt_ScoreDisplay,
                                        txt_ChanceDisplay,
                                        btn_NewGame,
                                        btn_Quit
                                    });
            Helper.SetFonts(Assets, QwertyList.Select(b => (View)b).ToList()); //set the fonts of the alphabetical buttons
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            ts.animTimer.Stop();
            ts.animTimer.Enabled = false;
            ts.Counter = 0;
            RunOnUiThread(() => { bckGround.SetImageResource(Resource.Drawable.BackGround); });
            var buttonEnabledArray = QwertyList.Select(b => b.Enabled).ToArray();

            outState.PutBooleanArray("buttonEnabledArray", buttonEnabledArray);
            outState.PutInt("difficulty", difficulty);
            outState.PutInt("score", score);
            outState.PutInt("chancesLeft", chancesLeft);
            outState.PutString("WordToGuess", WordToGuess);
            outState.PutString("WordDisplay", txt_WordDisplay.Text);
            base.OnSaveInstanceState(outState);
        }

        private void Btn_Quit_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
            Finish();
        }

        private void Btn_NewGame_Click(object sender, EventArgs e)
        {
            //this seems the best way to avoid triggering a restoration of state
            StartActivity(typeof(GameActivity)); 
            Finish();
        }

        private void LetterClicked(object sender, EventArgs e)
        {
            if (WordToGuess.ToLower().Contains((sender as Button).Text.ToLower()))
            {
                //code gets a little dense and abstract
                /*
                 * we need to create a new version of the displayed text where anything that hasn't yet
                 * been guessed is still hidden by an underscore and where each of the letters is separated 
                 * by a space if letters have been already guessed we need to preserve them into the new display
                 * and if new letters are guessed by the pressing of this button then we need to change the display
                 * to reflect that
                 */
                string tmp = "";
                int scoreBefore = score;
                for (int i = 0; i < WordToGuess.Length; i++)                                            //for every letter in the word to be guessed
                {
                    if (txt_WordDisplay.Text[i*2] == '_')                                               //check if we have already guessed it
                    {
                        if (WordToGuess[i].ToString().ToLower() == (sender as Button).Text.ToLower())   //  check if this is a successful guess in this spot
                        {
                            tmp = tmp + (sender as Button).Text.ToUpper() + ' ';                        //      preserve the spacing and use the button to store the correct guess
                            score += 10 * difficulty;
                            txt_ScoreDisplay.Text = "Score: " + score;
                        }
                        else                                                                            //  this was unsuccessful so just keep hidden and move on
                        {
                            tmp = tmp + "_ ";
                        }
                    }
                    else                                                                                //We have already guessed it just add it to the display
                    {
                        tmp = tmp + txt_WordDisplay.Text[i * 2] + ' ';
                    }
                }
                if (score - scoreBefore != 0)
                {
                    Helper.DisplayMessage(this, "Points: " + (score - scoreBefore));
                }
                txt_WordDisplay.Text = tmp;
                if (!tmp.Contains("_"))
                {
                    //all characters have now been correctly guessed
                    if (chancesLeft != 0) //don't exactly know if I should check this but best be carefull for now
                    {
                        txt_ChanceDisplay.SetTextSize(Android.Util.ComplexUnitType.Dip, txt_ChanceDisplay.TextSize + 2);
                        txt_ChanceDisplay.Text = "You Win";
                        StopPlaying();
                    }
                }
                //before leaving this we have to disable the button because there is no sense in letting the player guess the same guess
                (sender as Button).Enabled = false;
            }
            else
            {
                //the word didn't have this letter
                ts.animTimer.Enabled = true;
                ts.animTimer.Elapsed += AnimTimer_Elapsed;
                ts.animTimer.Start();
                chancesLeft--;
                txt_ChanceDisplay.Text = "Chances left: " + chancesLeft;
                if (chancesLeft == 0)
                {
                    txt_ChanceDisplay.SetTextSize(Android.Util.ComplexUnitType.Dip, txt_ChanceDisplay.TextSize + 2);
                    txt_ChanceDisplay.Text = "You Lose";
                    StopPlaying();
                }
                //#Rambling
                //Math.Abs(chancesLeft - HangingImageResource.Count)
                //this flips the number of chances into its inverse
                //on easy mode you get 9 chances and there will be 9 corresponding images saved into HangingImageResources
                //and so at the start of the game chancesLeft should be 9 and at this point we have passed chancesLeft-- so
                //and so we have to load the first image
                //which is 0 index so to get the 0 index based on the number nine you take away 9 or (chancesLeft - HangingImageResources.Count)
                //if you have a different difficulty then HangingImageResources.Count returns the chances for that difficulty
                RunOnUiThread(() => { img_Hang.SetImageResource(HangingImageResources[Math.Abs(chancesLeft - HangingImageResources.Count) - 1]); });
                (sender as Button).Enabled = false;

            }
        }

        private void AnimTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //using animationDrawable and viewAnimation and Animation never seemed to work
            //I'd get all sorts of errors or visual glitches
            //so an animation using a simple timer seemed the most likely way to get animation to work
            //btw this animation is just a little warning to the player that an error occured
            if (ts.Counter >= 25) //framerate of 20 per second 20 = 1 sec roughly
            {
                ts.animTimer.Stop();
                ts.animTimer.Enabled = false;
                ts.Counter = 0;
                RunOnUiThread(() => { bckGround.SetImageResource(Resource.Drawable.BackGround); });
                return;
            }
            ts.Counter++;
            if (ts.Counter % 2 == 0)
                RunOnUiThread(() => { bckGround.SetImageResource(Resource.Drawable.BackGround2); });
            else
                RunOnUiThread(() => { bckGround.SetImageResource(Resource.Drawable.BackGround); });
        }

        public void StopPlaying()
        {
            foreach (Button letterButton in QwertyList)
            {
                letterButton.Enabled = false;
            }
            txt_WordDisplay.Text = "Word was: \n" + string.Join("", WordToGuess.Select(l => l.ToString().ToUpper() + " ").ToArray());
            db.SaveNewHighscore(db.LoggedInAccount(this), score);
            btn_NewGame.Visibility = ViewStates.Visible;
            btn_Quit.Visibility = ViewStates.Visible;
        }
    }
}
