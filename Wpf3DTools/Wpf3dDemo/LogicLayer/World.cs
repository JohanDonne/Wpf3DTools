
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;
using Wpf3dDemo.Entities;

namespace Wpf3dDemo.LogicLayer;
internal class World : IWorld
{
    private const int _worldSize = 1000;

    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<IItem3D> Items { get; } = new();

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));

        CreateItems();
    }

    private void CreateItems()
    {
        Items.Add(new Cube { Position = new(100,100,0), Size=100});
        Items.Add(new Sphere { Position = new Point3D(250,100,0), Radius = 50 });
    }
}

