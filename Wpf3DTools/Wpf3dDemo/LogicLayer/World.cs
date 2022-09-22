
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;

namespace Wpf3dDemo.LogicLayer;
internal class World : IWorld
{
    private const int _worldSize = 1000;
    private readonly Random _rnd = new();

    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
    }
}

