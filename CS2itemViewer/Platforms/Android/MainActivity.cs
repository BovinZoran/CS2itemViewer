using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;

namespace CS2itemViewer
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Add this line to lock orientation to portrait
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

        }
    }
}
