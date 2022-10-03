using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record class Cone : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale { get; set; }

    public double Radius { get; }

    public Vector3D Axis { get; }

    public double YRotation => 0;

    public Cone(Point3D position, double radius, Vector3D axis, double scale = 1)
    {
        Position = position;
        Radius = radius;
        Axis = axis;
        Scale = scale;
    }
}
