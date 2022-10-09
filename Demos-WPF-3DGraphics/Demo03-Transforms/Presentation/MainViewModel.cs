using CommunityToolkit.Mvvm.ComponentModel;
using Demo03_Transforms.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Factories;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Demo03_Transforms.Presentation;
public class MainViewModel : ObservableObject
{

    private readonly World _world;
    private Model3DGroup _cubesGroup = new();
    private Model3DGroup _axesGroup = new();
    private bool _showAxes;
    private readonly ISphericalCameraController _cameraController;
    private readonly IShapesFactory _shapesFactory;

    public string Title => "Demo03: Coördinate transforms";

    public ProjectionCamera Camera => _cameraController.Camera;
    public Model3DGroup Visual3dContent { get; } = new();

    public bool? ShowAxes
    {
        get => _showAxes;
        set
        {
            if (value == _showAxes) return;
            _showAxes = value ?? false;
            if (_showAxes)
            {
                Visual3dContent.Children.Add(_axesGroup);
            }
            else
            {
                Visual3dContent.Children.Remove(_axesGroup);
            }
        }
    }

    public MainViewModel() : this(new World(), new SphericalCameraController(), new ShapesFactory()) { }

    public MainViewModel(World world, ISphericalCameraController cameraController, IShapesFactory shapesFactory)
    {
        _world = world;
        _cameraController = cameraController;
        _shapesFactory = shapesFactory;
        Init3DPresentation();
        SetupContent();
        _shapesFactory = shapesFactory;
    }

    private void Init3DPresentation()
    {
        SetupControllableCamera();
        SetUpLighting();
        CreateAxesGroup();
        ShowAxes = true;
    }

    private void SetUpLighting()
    {
        Visual3dContent.Children.Add(new AmbientLight(Colors.Gray));
        var direction = new Vector3D(1.5, -3, -5);
        Visual3dContent.Children.Add(new DirectionalLight(Colors.Gray, direction));
    }

    private void CreateAxesGroup()
    {
        double xLength = Math.Abs(_world.Bounds.p2.X - _world.Bounds.p1.X) / 2;
        double yLength = Math.Abs(_world.Bounds.p2.Y - _world.Bounds.p1.Y) / 2;
        double zLength = Math.Abs(_world.Bounds.p2.Z - _world.Bounds.p1.Z) / 2;
        double thickness = _world.Size / 200;
        _axesGroup = new Model3DGroup();
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }

    private void SetupContent()
    {
        CreateCubes();
        CreateGroundPlane();
    }

    private void CreateCubes()
    {
        var brush = new SolidColorBrush(Colors.SteelBlue);
        var material = new MaterialGroup();
        material.Children.Add(new DiffuseMaterial(brush));
        material.Children.Add(new SpecularMaterial(brush, 100));
        foreach (var cube in _world.Cubes)
        {
            var cubeViewModel = _shapesFactory.CreateCube(material);
            cubeViewModel.Transform = GetCubeTransForm(cube);
            _cubesGroup.Children.Add(cubeViewModel);
        }
        Visual3dContent.Children.Add(_cubesGroup);
    }

    private Transform3D GetCubeTransForm(Cube cube)
    {
        var transform = new Transform3DGroup();
        transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), cube.Rotation)));
        transform.Children.Add(new ScaleTransform3D(cube.Size, cube.Size, cube.Size));
        transform.Children.Add(new TranslateTransform3D(cube.Position - _world.Origin));
        return transform;
    }

    private void CreateGroundPlane()
    {
        var brush = new SolidColorBrush(Colors.Lime);
        var matGroup = new MaterialGroup();
        matGroup.Children.Add(new DiffuseMaterial(brush));
        matGroup.Children.Add(new SpecularMaterial(brush, 100));
        var plane = new PlaneViewModel(_world.Size, _world.Size / 3, matGroup);

        // To disable backface culling: add a Backface material to the Geometry
        // plane.Geometry.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray)); 

        Visual3dContent.Children.Add(plane.Geometry);
    }



    #region Camera control

    private void SetupControllableCamera()
    {
        double l1 = (_world.Bounds.p1 - _world.Origin).Length;
        double l2 = (_world.Bounds.p2 - _world.Origin).Length;
        double radius = 2.3 * Math.Max(l1, l2);
        _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
    }

    public void ProcessKey(Key key)
    {
        _cameraController.ControlByKey(key);
    }

    public void Zoom(int Delta)
    {
        _cameraController.Zoom(Delta);
    }

    public void ControlByMouse(Vector vector)
    {
        _cameraController.ControlByMouse(vector);
    }


    #endregion Camera control


}
