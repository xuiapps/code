namespace Xui.Core.DI;

/// <summary>
/// Provides information about the current device and platform.
/// Resolved via <c>this.GetService&lt;IDeviceInfo&gt;()</c> from any <see cref="Xui.Core.UI.View"/>.
/// The emulator replaces this with a mock to simulate phone or tablet form factors.
/// </summary>
public interface IDeviceInfo
{
    /// <summary>Gets the operating system the app is running on.</summary>
    DevicePlatform Platform { get; }

    /// <summary>Gets the form factor of the current device.</summary>
    DeviceFormFactor FormFactor { get; }
}

/// <summary>The operating system platform.</summary>
public enum DevicePlatform
{
    /// <summary>Microsoft Windows.</summary>
    Windows,
    /// <summary>Apple macOS.</summary>
    MacOS,
    /// <summary>Web browser (WASM).</summary>
    Browser,
    /// <summary>Apple iOS.</summary>
    iOS,
    /// <summary>Google Android.</summary>
    Android,
}

/// <summary>The physical form factor of the device.</summary>
public enum DeviceFormFactor
{
    /// <summary>Desktop or laptop computer.</summary>
    Desktop,
    /// <summary>Handheld phone.</summary>
    Mobile,
    /// <summary>Tablet device.</summary>
    Tablet,
}
