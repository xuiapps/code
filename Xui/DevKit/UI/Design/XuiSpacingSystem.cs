namespace Xui.DevKit.UI.Design;

/// <summary>
/// Concrete spacing system based on a 4-pt grid.
/// </summary>
internal class XuiSpacingSystem : ISpacingSystem
{
    /// <inheritdoc/>
    public nfloat None => 0;

    /// <inheritdoc/>
    public nfloat XXS => 2;

    /// <inheritdoc/>
    public nfloat XS => 4;

    /// <inheritdoc/>
    public nfloat S => 8;

    /// <inheritdoc/>
    public nfloat M => 12;

    /// <inheritdoc/>
    public nfloat L => 16;

    /// <inheritdoc/>
    public nfloat XL => 24;

    /// <inheritdoc/>
    public nfloat XXL => 32;

    /// <inheritdoc/>
    public nfloat XXXL => 48;
}
