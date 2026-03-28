using Xui.Core.Math3D;

namespace Xui.Core.Math3D;

public class Matrix4x4Test
{
    [Fact]
    public void Matrix4x4_Identity()
    {
        var m = Matrix4x4.Identity;
        Assert.True(m.IsIdentity);
        Assert.Equal(1f, m.M11);
        Assert.Equal(0f, m.M12);
        Assert.Equal(1f, m.M22);
        Assert.Equal(1f, m.M33);
        Assert.Equal(1f, m.M44);
    }

    [Fact]
    public void Matrix4x4_IdentityTransformsPointUnchanged()
    {
        var p = new Point3(1f, 2f, 3f);
        var result = Matrix4x4.Identity * p;
        Assert.Equal(1f, result.X, 1e-6f);
        Assert.Equal(2f, result.Y, 1e-6f);
        Assert.Equal(3f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_IdentityTransformsVectorUnchanged()
    {
        var v = new Vector3(1f, 2f, 3f);
        var result = Matrix4x4.Identity * v;
        Assert.Equal(1f, result.X, 1e-6f);
        Assert.Equal(2f, result.Y, 1e-6f);
        Assert.Equal(3f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_Translate()
    {
        var m = Matrix4x4.Translate(new Vector3(5f, 3f, 1f));
        var result = m * new Point3(1f, 1f, 1f);
        Assert.Equal(6f, result.X, 1e-6f);
        Assert.Equal(4f, result.Y, 1e-6f);
        Assert.Equal(2f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_TranslateDoesNotAffectVectors()
    {
        var m = Matrix4x4.Translate(new Vector3(5f, 3f, 1f));
        var v = new Vector3(1f, 2f, 3f);
        var result = m * v;
        // Translation should not change vectors (no w component contribution)
        Assert.Equal(1f, result.X, 1e-6f);
        Assert.Equal(2f, result.Y, 1e-6f);
        Assert.Equal(3f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_ScaleUniform()
    {
        var m = Matrix4x4.Scale(2f);
        var result = m * new Point3(1f, 2f, 3f);
        Assert.Equal(2f, result.X, 1e-6f);
        Assert.Equal(4f, result.Y, 1e-6f);
        Assert.Equal(6f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_ScaleNonUniform()
    {
        var m = Matrix4x4.Scale(new Vector3(1f, 2f, 3f));
        var result = m * new Point3(2f, 2f, 2f);
        Assert.Equal(2f, result.X, 1e-6f);
        Assert.Equal(4f, result.Y, 1e-6f);
        Assert.Equal(6f, result.Z, 1e-6f);
    }

    [Fact]
    public void Matrix4x4_RotateX_90Degrees()
    {
        var m = Matrix4x4.RotateX(MathF.PI / 2f);
        var result = m * Vector3.Up;
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_RotateY_90Degrees()
    {
        var m = Matrix4x4.RotateY(MathF.PI / 2f);
        var result = m * Vector3.Right;
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(-1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_RotateZ_90Degrees()
    {
        var m = Matrix4x4.RotateZ(MathF.PI / 2f);
        var result = m * Vector3.Right;
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(1f, result.Y, 1e-5f);
        Assert.Equal(0f, result.Z, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_RotateFromQuaternion()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var m = Matrix4x4.Rotate(q);
        var result = m * Vector3.Right;
        // +90° around Y rotates Right → Forward (-Z), consistent with RotateY.
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(-1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_Compose_TranslateAndScale()
    {
        // Scale then translate: first scale (2x), then translate by (1,0,0).
        var translate = Matrix4x4.Translate(new Vector3(1f, 0f, 0f));
        var scale = Matrix4x4.Scale(2f);
        var combined = translate * scale;   // scale is applied first

        var result = combined * new Point3(1f, 0f, 0f);
        // Scaled: (2, 0, 0), then translated: (3, 0, 0)
        Assert.Equal(3f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(0f, result.Z, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_Determinant_Identity()
    {
        Assert.Equal(1f, Matrix4x4.Identity.Determinant, 1e-5f);
    }

    [Fact]
    public void Matrix4x4_Inverse_Identity()
    {
        var inv = Matrix4x4.Identity.Inverse;
        Assert.True(inv.IsIdentity);
    }

    [Fact]
    public void Matrix4x4_Inverse_Translation()
    {
        var m = Matrix4x4.Translate(new Vector3(3f, 2f, 1f));
        var inv = m.Inverse;
        var combined = m * inv;

        Assert.Equal(Matrix4x4.Identity.M11, combined.M11, 1e-4f);
        Assert.Equal(Matrix4x4.Identity.M14, combined.M14, 1e-4f);
        Assert.Equal(Matrix4x4.Identity.M24, combined.M24, 1e-4f);
        Assert.Equal(Matrix4x4.Identity.M34, combined.M34, 1e-4f);
    }

    [Fact]
    public void Matrix4x4_Transpose()
    {
        var m = new Matrix4x4(
            1f, 2f, 3f, 4f,
            5f, 6f, 7f, 8f,
            9f, 10f, 11f, 12f,
            13f, 14f, 15f, 16f
        );
        var t = m.Transposed;
        Assert.Equal(m.M12, t.M21);
        Assert.Equal(m.M13, t.M31);
        Assert.Equal(m.M14, t.M41);
        Assert.Equal(m.M23, t.M32);
    }

    [Fact]
    public void Matrix4x4_Equality()
    {
        var a = Matrix4x4.Identity;
        var b = Matrix4x4.Identity;
        Assert.True(a == b);
        Assert.False(a == Matrix4x4.Scale(2f));
        Assert.True(a != Matrix4x4.Scale(2f));
    }
}
