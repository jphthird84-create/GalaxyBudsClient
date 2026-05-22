using System;
using System.Numerics;
using GalaxyBudsClient.Models;
using GalaxyBudsClient.Models.SOFA;

namespace GalaxyBudsClient.Models;

public class SpatialAudioEngine
{
    private SpatialQuaternion _currentHeadOrientation = new(0, 0, 0, 1);
    private readonly HRTFProcessor _hrtfProcessor = new();
    private readonly object _lock = new();

    public event EventHandler<SpatialQuaternion>? HeadOrientationChanged;

    public void UpdateHeadOrientation(SpatialQuaternion quaternion)
    {
        lock (_lock)
        {
            _currentHeadOrientation = quaternion;
            var euler = QuaternionToEuler(quaternion);
            _hrtfProcessor.UpdateHRTF(euler.Yaw, euler.Pitch);
            HeadOrientationChanged?.Invoke(this, quaternion);
        }
    }

    private (float Yaw, float Pitch) QuaternionToEuler(SpatialQuaternion q)
    {
        float yaw = (float)(Math.Atan2(2 * (q.W * q.Y + q.X * q.Z), 1 - 2 * (q.Y * q.Y + q.Z * q.Z)) * 180 / Math.PI);
        float pitch = (float)(Math.Asin(2 * (q.W * q.X - q.Z * q.Y)) * 180 / Math.PI);
        return (yaw, pitch);
    }

    public (float left, float right) ProcessAudioSample(float monoSample)
    {
        return _hrtfProcessor.ProcessSample(monoSample);
    }

    public bool LoadHRTF(string sofaFilePath)
    {
        return HRTFLoader.LoadSOFA(sofaFilePath, out var left, out var right);
    }

    public SpatialQuaternion GetCurrentOrientation()
    {
        lock (_lock) return _currentHeadOrientation;
    }
}