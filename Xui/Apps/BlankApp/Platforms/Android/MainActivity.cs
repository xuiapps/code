using Android.App;
using Android.OS;
using Android.Content.PM;
using Xui.Runtime.Android.Actual;

namespace Xui.Apps.BlankApp;

[Activity(
    MainLauncher = true,
    Theme = "@style/Theme.AppCompat",
    LaunchMode = LaunchMode.SingleTop,
    Name = "xui.apps.blankapp.MainActivity",
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : XuiActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Xui.Apps.BlankApp.App.Main(new string[0]);
    }
}
