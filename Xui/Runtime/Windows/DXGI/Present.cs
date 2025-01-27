using System;

namespace Xui.Runtime.Windows;

public static partial class DXGI
{
    [Flags]
    public enum Present : uint
    {
        Test = 0x00000001U,
        DoNotSequence = 0x00000002U,
        Restart = 0x00000004U,
        DoNotWait = 0x00000008U,
        StereoPreferRight = 0x00000010U,
        StereoTemporaryMono = 0x00000020U,
        RestrictToOutput = 0x00000040U,
        UseDuration = 0x00000100U,
        AllowTearing = 0x00000200U
    }
}


