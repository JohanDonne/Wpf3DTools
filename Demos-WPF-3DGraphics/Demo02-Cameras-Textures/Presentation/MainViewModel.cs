using CommunityToolkit.Mvvm.ComponentModel;
using Demo02_Cameras_Textures.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Factories;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Demo02_Cameras_Textures.Presentation;

public class MainViewModel : ObservableObject
{

    private readonly World _world;
    private Model3DGroup _axesGroup = new();
    private AmbientLight? _ambientLight;
    private DirectionalLight? _directionalLight;
    private bool _showAxes;
    private bool _showAmbientLight;
    private bool _showDirectionalLight;
    private readonly ISphericalCameraController _cameraController;
    private readonly IShapesFactory _shapesFactory;

    public string Title => "Demo02: Controllable Camera & Textures";

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

    public bool? ShowAmbientLight
    {
        get => _showAmbientLight;
        set
        {
            if (value == _showAmbientLight) return;
            _showAmbientLight = value ?? false;
            if (_showAmbientLight)
            {
                Visual3dContent.Children.Add(_ambientLight);
            }
            else
            {
                Visual3dContent.Children.Remove(_ambientLight);
            }
        }
    }

    public bool? ShowDirectionalLight
    {
        get => _showDirectionalLight;
        set
        {
            if (value == _showDirectionalLight) return;
            _showDirectionalLight = value ?? false;
            if (_showDirectionalLight)
            {
                Visual3dContent.Children.Add(_directionalLight);
            }
            else
            {
                Visual3dContent.Children.Remove(_directionalLight);
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
        
    }

    private void Init3DPresentation()
    {
        SetupControllableCamera();
        SetUpLighting();
        CreateAxesGroup();
        ShowAxes = true;
        ShowAmbientLight = true;
        ShowDirectionalLight = true;
    }

    private void SetUpLighting()
    {
        _ambientLight = new AmbientLight(Colors.Gray);
        var direction = new Vector3D(1.5, -5, -5);
        _directionalLight = new DirectionalLight(Colors.White, direction);
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
        CreateCube();
        CreateGroundPlane();
    }

    private void CreateCube()
    {
        var brush = new SolidColorBrush(Colors.LightSteelBlue);
        var material = new MaterialGroup();
        material.Children.Add(new DiffuseMaterial(brush));
        material.Children.Add(new SpecularMaterial(brush, 100));
        var cube = new CubeViewModel(_world.Size / 5, material);
        Visual3dContent.Children.Add(cube.Geometry);
    }

    private void CreateGroundPlane()
    {
        var texture = (ImageSource)new ImageSourceConverter().ConvertFrom(Properties.Resources.Floor)!;
        var brush = new ImageBrush(texture);
        var material = new MaterialGroup();
        material.Children.Add(new DiffuseMaterial(brush));
        material.Children.Add(new SpecularMaterial(brush, 100));
        var plane = new PlaneViewModel(_world.Size, _world.Size / 3, material);

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
