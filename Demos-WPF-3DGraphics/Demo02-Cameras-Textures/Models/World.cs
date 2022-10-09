using System.Windows.Media.Media3D;

namespace Demo02_Cameras_Textures.Models;

public class World
{
    public double Size => 500;
    public Point3D Origin => new Point3D(0, 0, 0);
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public World()
    {
        Bounds = (new Point3D(-Size / 2, -Size / 2, -Size / 2),
                  new Point3D(Size / 2, Size / 2, Size / 2));
    }
}
