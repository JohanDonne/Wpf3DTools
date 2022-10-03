using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record class Cylinder : IItem3D
{
    public Point3D Position { get; set; }

    public double Scale => 1;

    public double YRotation => 0;

    public double Radius { get; init; }

    public Vector3D Axis { get; init; }

    public Cylinder(Point3D position, double radius, Vector3D axis)
    {
        Position = position;
        Radius = radius;
        Axis = axis;
    }
}
