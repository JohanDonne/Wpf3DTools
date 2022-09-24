using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;
using Wpf3dDemo.Entities;
using Wpf3dTools.Interfaces;

namespace Wpf3dDemo.PresentationLayer;
public class MainViewModel
{
    private readonly IWorld _world;
    private readonly ISphericalCameraController _cameraController;
    private readonly ILineFactory _lineFactory;
    private readonly ICubeFactory _cubeFactory;
    private readonly ISphereFactory _sphereFactory;
    private readonly SolidColorBrush[] _colorBrushList = new SolidColorBrush[]
       {
            new SolidColorBrush(Colors.MediumBlue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.DarkOrange),
            new SolidColorBrush(Colors.Olive),
            new SolidColorBrush(Colors.DarkCyan),
            new SolidColorBrush(Colors.Brown),
            new SolidColorBrush(Colors.SteelBlue),
            new SolidColorBrush(Colors.Gold),
            new SolidColorBrush(Colors.MistyRose),
            new SolidColorBrush(Colors.PaleTurquoise),
            new SolidColorBrush(Colors.PeachPuff),
            new SolidColorBrush(Colors.Salmon),
            new SolidColorBrush(Colors.Silver),
       };

    private readonly Model3DGroup _model3dGroup = new();
    private readonly Model3DGroup _axesGroup = new();
    private readonly Model3DGroup _itemsGroup = new();
    private bool _showAxes;

    public ProjectionCamera Camera => _cameraController.Camera;
    public Model3D Visual3dContent => _model3dGroup;

    private readonly List<GeometryModel3D> _itemsList = new();

    public bool? ShowAxes
    {
        get => _showAxes;
        set
        {
            if (value == _showAxes) return;
            _showAxes = value ?? false;
            if (_showAxes)
            {
                _model3dGroup.Children.Add(_axesGroup);
            }
            else
            {
                _model3dGroup.Children.Remove(_axesGroup);
            }
        }
    }

    public MainViewModel
        (
            IWorld world,
            ISphericalCameraController cameraController,
            ILineFactory lineFactory,
            ICubeFactory cubeFactory,
            ISphereFactory sphereFactory
        )
    {
        _world = world;
        _cameraController = cameraController;
        _lineFactory = lineFactory;
        _cubeFactory = cubeFactory;
        _sphereFactory = sphereFactory;

        Init3DPresentation();
        InitItemGeometries();
        UpdateWorldDisplay();       
    }

    private void UpdateWorldDisplay()
    {
        for (int i = 0; i < _itemsList.Count; i++)
        {
            var _itemTransform = new Transform3DGroup();
            _itemTransform.Children.Add(new ScaleTransform3D(_world.Items[i].Scale, _world.Items[i].Scale, _world.Items[i].Scale));
            _itemTransform.Children.Add(new TranslateTransform3D(_world.Items[i].Position - _world.Origin));
            _itemsList[i].Transform = _itemTransform;
        };
    }

    #region 3D initialisation

    private void InitItemGeometries()
    {
        foreach (var item in _world.Items)
        {
            var geometry = item switch
            {
                Cube => _cubeFactory.Create(GetMaterial(0)),
                Sphere => _sphereFactory.Create(GetMaterial(1)),
                _ => throw new ArgumentException("Unknown type of a item"),
            };
            _itemsList.Add(geometry);
            _itemsGroup.Children.Add(geometry);
        }
        _model3dGroup.Children.Add(_itemsGroup);
    }

    private void Init3DPresentation()
    {
        SetupCamera();
        SetUpLights();
        CreateAxesGroup();
        ShowAxes = true;
    }

    private void SetUpLights()
    {
        _model3dGroup.Children.Add(new AmbientLight(Colors.Gray));
        var direction = new Vector3D(1.5, -3, -5);
        _model3dGroup.Children.Add(new DirectionalLight(Colors.Gray, direction));
    }

    private void CreateAxesGroup()
    {
        double xLength = Math.Abs(_world.Bounds.p2.X - _world.Bounds.p1.X) / 2;
        double yLength = Math.Abs(_world.Bounds.p2.Y - _world.Bounds.p1.Y) / 2;
        double zLength = Math.Abs(_world.Bounds.p2.Z - _world.Bounds.p1.Z) / 2;
        double thickness = (_world.Bounds.p2 - _world.Bounds.p1).Length / 500;
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }

    private MaterialGroup GetMaterial(int index)
    {
        var brush = _colorBrushList[index];
        var matGroup = new MaterialGroup();
        matGroup.Children.Add(new DiffuseMaterial(brush));
        matGroup.Children.Add(new SpecularMaterial(brush, 100));
        return matGroup;
    }

    #endregion 3D initialisation

    #region Camera control

    private void SetupCamera()
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

    public void Zoom(int delta)
    {
        _cameraController.Zoom(delta);
    }

    public void ControlByMouse(Vector vector)
    {
        _cameraController.ControlByMouse(vector);
    }

    #endregion Camera control
}
