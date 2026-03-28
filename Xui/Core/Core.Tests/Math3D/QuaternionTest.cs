using Xui.Core.Math3D;

namespace Xui.Core.Math3D;

public class QuaternionTest
{
    [Fact]
    public void Quaternion_Identity()
    {
        var id = Quaternion.Identity;
        Assert.Equal(0f, id.X);
        Assert.Equal(0f, id.Y);
        Assert.Equal(0f, id.Z);
        Assert.Equal(1f, id.W);
        Assert.Equal(1f, id.Magnitude, 1e-6f);
    }

    [Fact]
    public void Quaternion_IdentityRotatesNothing()
    {
        var v = new Vector3(1f, 2f, 3f);
        var result = Quaternion.Identity * v;
        Assert.Equal(v.X, result.X, 1e-5f);
        Assert.Equal(v.Y, result.Y, 1e-5f);
        Assert.Equal(v.Z, result.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_FromAxisAngle_IsUnit()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 4f);
        Assert.Equal(1f, q.Magnitude, 1e-6f);
    }

    [Fact]
    public void Quaternion_RotateAroundY_90Degrees()
    {
        // Rotating Right (1, 0, 0) by +90° around Y (CCW from above) gives Forward (0, 0, -1).
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var result = q * Vector3.Right;
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(-1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_RotateAroundX_90Degrees()
    {
        // Rotating Up (0, 1, 0) by 90° around X should give Back (0, 0, 1).
        var q = Quaternion.FromAxisAngle(Vector3.Right, MathF.PI / 2f);
        var result = q * Vector3.Up;
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_Conjugate()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 3f);
        var c = q.Conjugate;
        Assert.Equal(-q.X, c.X, 1e-6f);
        Assert.Equal(-q.Y, c.Y, 1e-6f);
        Assert.Equal(-q.Z, c.Z, 1e-6f);
        Assert.Equal(q.W, c.W, 1e-6f);
    }

    [Fact]
    public void Quaternion_ConjugateUndoesRotation()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 4f);
        var v = new Vector3(1f, 0f, 0f);
        var rotated = q * v;
        var restored = q.Conjugate * rotated;
        Assert.Equal(v.X, restored.X, 1e-5f);
        Assert.Equal(v.Y, restored.Y, 1e-5f);
        Assert.Equal(v.Z, restored.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_Multiply_Compose()
    {
        // Two 90° rotations around Y should equal one 180° rotation.
        var q90 = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var q180 = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI);
        var composed = q90 * q90;

        var v = Vector3.Right;
        var expected = q180 * v;
        var actual = composed * v;

        Assert.Equal(expected.X, actual.X, 1e-5f);
        Assert.Equal(expected.Y, actual.Y, 1e-5f);
        Assert.Equal(expected.Z, actual.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_Normalized()
    {
        var q = new Quaternion(1f, 2f, 3f, 4f);
        var n = q.Normalized;
        Assert.Equal(1f, n.Magnitude, 1e-6f);
    }

    [Fact]
    public void Quaternion_Lerp()
    {
        var a = Quaternion.Identity;
        var b = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var mid = Quaternion.Lerp(a, b, 0.5f);
        Assert.Equal(1f, mid.Magnitude, 1e-6f);
    }

    [Fact]
    public void Quaternion_Slerp_AtZero()
    {
        var a = Quaternion.Identity;
        var b = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var result = Quaternion.Slerp(a, b, 0f);
        var v = Vector3.Right;
        var rotated = result * v;
        Assert.Equal(1f, rotated.X, 1e-5f);
        Assert.Equal(0f, rotated.Y, 1e-5f);
        Assert.Equal(0f, rotated.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_Slerp_AtOne()
    {
        var a = Quaternion.Identity;
        var b = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var result = Quaternion.Slerp(a, b, 1f);
        var v = Vector3.Right;
        var rotated = result * v;
        // +90° around Y rotates Right → Forward (-Z).
        Assert.Equal(0f, rotated.X, 1e-5f);
        Assert.Equal(0f, rotated.Y, 1e-5f);
        Assert.Equal(-1f, rotated.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_ToMatrix_IdentityGivesIdentityMatrix()
    {
        var m = Quaternion.Identity.ToMatrix4x4();
        Assert.True(m.IsIdentity);
    }

    [Fact]
    public void Quaternion_ToMatrix_RotateAroundY_90Degrees()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 2f);
        var m = q.ToMatrix4x4();
        var result = m * Vector3.Right;
        // +90° around Y rotates Right → Forward (-Z).
        Assert.Equal(0f, result.X, 1e-5f);
        Assert.Equal(0f, result.Y, 1e-5f);
        Assert.Equal(-1f, result.Z, 1e-5f);
    }

    [Fact]
    public void Quaternion_ToEuler_RoundTrip()
    {
        float pitch = 0.3f, yaw = 0.5f, roll = 0.1f;
        var q = Quaternion.FromEuler(pitch, yaw, roll);
        var (ep, ey, er) = q.ToEuler();
        Assert.Equal(pitch, ep, 1e-5f);
        Assert.Equal(yaw, ey, 1e-5f);
        Assert.Equal(roll, er, 1e-5f);
    }

    [Fact]
    public void Quaternion_NumericsInterop()
    {
        var q = Quaternion.FromAxisAngle(Vector3.Up, MathF.PI / 4f);
        System.Numerics.Quaternion nq = q;
        Assert.Equal(q.X, nq.X, 1e-6f);
        Assert.Equal(q.Y, nq.Y, 1e-6f);
        Assert.Equal(q.Z, nq.Z, 1e-6f);
        Assert.Equal(q.W, nq.W, 1e-6f);

        Quaternion back = nq;
        Assert.Equal(q, back);
    }
}
