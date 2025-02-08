using Android.Content;

namespace Xui.Runtime.Android.Actual;

public class XuiRoot : global::Android.Views.ViewGroup
{
    protected readonly XuiActivity xuiActivity;

    public XuiRoot(XuiActivity xuiActivity) : base(xuiActivity)
    {
        this.xuiActivity = xuiActivity;
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        for (int i = 0; i < this.ChildCount; i++)
        {
            var child = this.GetChildAt(i)!;
            child.Layout(l, t, r, b);
        }
    }
}