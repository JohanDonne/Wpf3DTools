
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;
using Wpf3dDemo.Entities;

namespace Wpf3dDemo.LogicLayer;
internal class World : IWorld
{
    private const int _worldSize = 1000;
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));
    private Parallelogram? _rectangle;
    private Circle? _circle;

    public Point3D Origin => new();
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<IItem3D> Items { get; } = new();

    public Snowman Snowman { get; private set; }

    public World()
    {
        Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
        CreateItems();
        Snowman = new Snowman(position: new(-500, 0, 0), 1);
        Task.Run(async () =>
        {
            while (true)
            {
                await _timer.WaitForNextTickAsync();
                UpdateWorld();
            }
        });
    }

    private void UpdateWorld()
    {
        Snowman.YRotation += 1;
        _rectangle!.YRotation -= 2;
        _circle!.YRotation += 3;
    }

    private void CreateItems()
    {
        Items.Add(new Cube(position: new(100, 100, 0), size: 100));
        Items.Add(new Sphere(position: new(250, 100, 0), radius: 50));
        Items.Add(new Beam(position: new Point3D(350, 75, 50), xSize: 50, ySize: 80, zSize: 200));
        Items.Add(new Cylinder(position: new(500, 50, 50), radius: 30, axis: new(0, 150, -100)));
        Items.Add(new Cone(position: new(650, 50, -50), radius: 40, axis: new(0, 150, 100)));
        _rectangle = new Parallelogram(origin: new(-50, 100, 0), side1: new(-80, 0, 50), new(0, 100, -20));
        Items.Add(_rectangle);
        _circle = new Circle(center: new Point3D(-250, 100, 0), radius: 50, normal: new(0, 1, 1));
        Items.Add(_circle);
    }
}

