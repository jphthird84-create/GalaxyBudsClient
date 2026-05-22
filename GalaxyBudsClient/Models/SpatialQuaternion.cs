namespace GalaxyBudsClient.Models;

public struct SpatialQuaternion
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public SpatialQuaternion(float x, float y, float z, float w)
    {
        X = x; Y = y; Z = z; W = w;
    }

    public override string ToString() => $"X:{X:F4}, Y:{Y:F4}, Z:{Z:F4}, W:{W:F4}";
}