using System.Windows.Media.Media3D;

namespace Demo04_Movement.Models;
public record Cube
{
    public double Size { get; init; }
    public Point3D Position { get; init; }
    public double Rotation { get; set; }
    public double RotationsPerMinute { get;init; }

    public void Rotate(TimeSpan ellapsedTime)
    {
        Rotation += ellapsedTime.TotalMinutes * RotationsPerMinute * 360;
    }
}
