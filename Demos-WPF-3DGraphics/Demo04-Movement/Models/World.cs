using System.Windows.Media.Media3D;

namespace Demo04_Movement.Models;
public class World
{
    private CancellationTokenSource? _cancellationTokenSource;

    public double Size => 500;
    public Point3D Origin => new Point3D(0, 0, 0);
    public (Point3D p1, Point3D p2) Bounds { get; private set; }

    public List<Cube> Cubes { get; private set; }

    public Sphere Sphere { get; private set; }

    public World()
    {
        Bounds = (new Point3D(-Size / 2, -Size / 2, -Size / 2),
                  new Point3D(Size / 2, Size / 2, Size / 2));
        Cubes = CreateCubes()!;
        Sphere = new Sphere(
                         radius: Size / 15,
                         position: new Point3D(Size * 0.3, Size / 3, 0),
                         rotationsPerMinute: -30
            );
    }

    private List<Cube>? CreateCubes()
    {
        const int dimension = 5;
        Point3D startPosition = new(-Size * 0.3, 0, Size * 0.3);
        double delta = Size * 0.6 / (dimension - 1);
        double size = delta * 0.5;
        var xDelta = new Vector3D(delta, 0, 0);
        var zDelta = new Vector3D(0, 0, -delta);
        double rotation = 0;
        return CreateCubes(dimension, startPosition, size, rotation, xDelta, zDelta);
    }

    private List<Cube> CreateCubes(int dimension, Point3D startPosition, double size, double rotation, Vector3D xDelta, Vector3D zDelta)
    {
        var rnd = new Random();
        var list = new List<Cube>();
        for (int x = 0; x < dimension; x++)
        {
            for (int z = 0; z < dimension; z++)
            {
                var cube = new Cube
                {
                    Position = startPosition + x * xDelta + z * zDelta,
                    Size = size,
                    Rotation = rotation,
                    RotationsPerMinute = 5 + 55 * rnd.NextDouble()
                };
                list.Add(cube);
            }
        }
        return list;
    }

    public void StartMovement()
    {
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        Task.Run(() => ExecuteSimulationLoop(token), token);
    }

    public void StopMovement()
    {
        _cancellationTokenSource?.Cancel();
    }

    private void ExecuteSimulationLoop(CancellationToken cancelToken)
    {
        DateTime previousTime = DateTime.Now;
        while (!cancelToken.IsCancellationRequested)
        {
            var ellapsedTime = DateTime.Now - previousTime;
            previousTime = DateTime.Now;
            Sphere.RotateAroundY(ellapsedTime);
            foreach (var cube in Cubes)
            {
                cube.Rotate(ellapsedTime);
            }
        }
    }


}
