using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace HangmanApp
{
    public class Helper
    {
        private static Toast singleToast;
        public static void DisplayMessage(Context _context, string message, ToastLength t = ToastLength.Short)
        {
            if (singleToast != null)
            {
                singleToast.Cancel();
            }
            if (!string.IsNullOrEmpty(message) && _context != null)
            {
                singleToast = Toast.MakeText(_context, message, t);
                singleToast.Show();
            }
        }
        public static void SetFonts(AssetManager Assets,List<View> controls)
        {
            var font = Typeface.CreateFromAsset(Assets, "iosevka-regular.ttf");
            foreach (View V in controls)
            {
                (V as TextView).Typeface = font;
            }
        }
        public static string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = System.IO.Path.Combine(path, filename);

            CopyDatabaseIfNotExists(dbPath);

            return dbPath;
        }
        private static void CopyDatabaseIfNotExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                using (var br = new BinaryReader(Application.Context.Assets.Open("words.db3")))
                {
                    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }
        }
    }
}