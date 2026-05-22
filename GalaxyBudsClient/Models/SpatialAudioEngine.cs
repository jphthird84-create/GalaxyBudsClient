using System;
using System.Numerics;
using GalaxyBudsClient.Models;

namespace GalaxyBudsClient.Models;

public class SpatialAudioEngine
{
    private SpatialQuaternion _currentHeadOrientation = new SpatialQuaternion(0, 0, 0, 1);
    private readonly object _lock = new object();

    public event EventHandler<SpatialQuaternion>? HeadOrientationChanged;

    public void UpdateHeadOrientation(SpatialQuaternion quaternion)
    {
        lock (_lock)
        {
            _currentHeadOrientation = quaternion;
            HeadOrientationChanged?.Invoke(this, quaternion);
        }
    }

    public SpatialQuaternion GetCurrentOrientation()
    {
        lock (_lock)
        {
            return _currentHeadOrientation;
        }
    }

    // Simple rotation matrix for audio spatialization (can be extended with HRTF later)
    public Vector3 RotateAudioSource(Vector3 sourcePosition)
    {
        var q = GetCurrentOrientation();
        var rotation = Quaternion.CreateFromRotationMatrix(Matrix4x4.CreateFromQuaternion(new Quaternion(q.X, q.Y, q.Z, q.W)));
        return Vector3.Transform(sourcePosition, rotation);
    }

    public override string ToString() => $"SpatialAudioEngine - Current: {_currentHeadOrientation}";
}