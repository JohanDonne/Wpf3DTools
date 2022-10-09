using System.Windows.Media.Media3D;

namespace Demo03_Transforms.Models;

public record Cube
{
    public double Size { get; init; }
    public Point3D Position { get; init; }
    public double Rotation { get; init; }
}
