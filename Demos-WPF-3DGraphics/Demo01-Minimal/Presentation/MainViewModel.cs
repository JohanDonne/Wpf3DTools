using CommunityToolkit.Mvvm.ComponentModel;
using Demo01_Minimal.Models;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Demo01_Minimal.Presentation;

public class MainViewModel : ObservableObject
{

    private readonly World _world;
    private Model3DGroup _axesGroup = new();

    public string Title => "Demo01: Minimal 3D Graphics";

    public ProjectionCamera Camera { get; private set; }
    public Model3DGroup Visual3dContent { get; } = new();


    public MainViewModel() : this(new World()) { }

    public MainViewModel(World world)
    {
        _world = world;
        Camera = SetupCamera();
        SetUpLighting();
        SetupContent();
    }

    private ProjectionCamera SetupCamera()
    {
        var position = new Point3D(_world.Size, _world.Size, _world.Size);
        return new PerspectiveCamera()
        {
            Position = position,
            LookDirection = _world.Origin - position,
            UpDirection = new Vector3D(0, 1, 0)
        };
    }

    private void SetUpLighting()
    {
        Visual3dContent.Children.Add(new AmbientLight(Colors.Gray));
        var direction = new Vector3D(1.5, -3, -5);
        Visual3dContent.Children.Add(new DirectionalLight(Colors.Gray, direction));
    }

    private void SetupContent()
    {
        var brush = new SolidColorBrush(Colors.SteelBlue);
        var material = new MaterialGroup();
        material.Children.Add(new DiffuseMaterial(brush));
        material.Children.Add(new SpecularMaterial(brush, 100));

        var cube = new CubeViewModel(100, material);
        Visual3dContent.Children.Add(cube.Geometry);
    }
}
