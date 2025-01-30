using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Platform;

namespace Xui.Apps;

[Activity(
    MainLauncher = true,
    Theme = "@style/Theme.AppCompat",
    LaunchMode = LaunchMode.SingleTop,
    Name = "com.xuiapps.XuiMainActivity",
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class XuiMainActivity : AndroidX.AppCompat.App.AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        this.SupportActionBar?.Hide();

        base.OnCreate(savedInstanceState);

        var windowFlags = Android.Views.WindowManagerFlags.LayoutNoLimits;
        this.Window!.SetFlags(windowFlags, windowFlags);
        var ui = new Android.Views.View(this);
        this.SetContentView(ui);

        // TODO: Implement canvas inside a custom View class
        // TODO: For Vulcan - add a root group view with Vulcan and transparent UI view overlay
    }
}
