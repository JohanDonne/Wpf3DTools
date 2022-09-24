using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record struct Snowman : IItem3D
{
    public Point3D Position { get; set; }
    public double Scale { get; set; }

    public Snowman(Point3D position, double scale)
    {
        Position = position;
        Scale = scale;
    }
}
