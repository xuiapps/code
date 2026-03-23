namespace Xui.DevKit.UI.Design;

/// <summary>
/// Provides spacing tokens based on a 4-pt grid.
/// </summary>
public interface ISpacingSystem
{
    /// <summary>0 pt.</summary>
    nfloat None { get; }

    /// <summary>2 pt.</summary>
    nfloat XXS { get; }

    /// <summary>4 pt.</summary>
    nfloat XS { get; }

    /// <summary>8 pt.</summary>
    nfloat S { get; }

    /// <summary>12 pt.</summary>
    nfloat M { get; }

    /// <summary>16 pt.</summary>
    nfloat L { get; }

    /// <summary>24 pt.</summary>
    nfloat XL { get; }

    /// <summary>32 pt.</summary>
    nfloat XXL { get; }

    /// <summary>48 pt.</summary>
    nfloat XXXL { get; }
}
