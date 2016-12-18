using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using droid = Android;

namespace Client.Android
{
    [Activity(Label = "",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class MainActivity : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            var g = new GameClient(Assets);
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
        }

    }
}