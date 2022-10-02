using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;
using Wpf3dDemo.Entities;
using Wpf3dTools.Interfaces;

namespace Wpf3dDemo.PresentationLayer;
public class MainViewModel : ObservableObject
{
    private readonly IWorld _world;
    private readonly ISphericalCameraController _cameraController;
    private readonly IShapesFactory _shapesFactory;

    private readonly Color[] _colorList = new Color[]
       {
            Colors.MediumBlue,
            Colors.Green,
            Colors.DarkOrange,
            Colors.Olive,
            Colors.DarkCyan,
            Colors.Brown,
            Colors.SteelBlue,
            Colors.Gold,
            Colors.MistyRose,
            Colors.PaleTurquoise,
            Colors.PeachPuff,
            Colors.Salmon,
            Colors.Silver,
       };

    private readonly Model3DGroup _model3dGroup = new();
    private readonly Model3DGroup _axesGroup = new();
    private readonly Model3DGroup _itemsGroup = new();
    private readonly Model3DGroup _snowmanGroup = new();
    private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(10));
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

    public MainViewModel(IWorld world, ISphericalCameraController cameraController, IShapesFactory shapesFactory)
    {
        _world = world;
        _cameraController = cameraController;
        _shapesFactory = shapesFactory;

        Init3DPresentation();
        InitItemGeometries();
        _ = Animate();
    }

    public async Task Animate()
    {
        while (true)
        {
            UpdateWorldDisplay();
            await _timer.WaitForNextTickAsync();
        }
    }

    private void UpdateWorldDisplay()
    {
        for (int i = 0; i < _itemsList.Count; i++)
        {
            var itemTransform = new Transform3DGroup();
            itemTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.Items[i].YRotation)));
            itemTransform.Children.Add(new ScaleTransform3D(_world.Items[i].Scale, _world.Items[i].Scale, _world.Items[i].Scale));
            itemTransform.Children.Add(new TranslateTransform3D(_world.Items[i].Position - _world.Origin));
            _itemsList[i].Transform = itemTransform;
        };
        var snowmanTransform = new Transform3DGroup();
        snowmanTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.Snowman.YRotation)));
        snowmanTransform.Children.Add(new ScaleTransform3D(_world.Snowman.Scale, _world.Snowman.Scale, _world.Snowman.Scale));
        snowmanTransform.Children.Add(new TranslateTransform3D(_world.Snowman.Position - _world.Origin));
        _snowmanGroup.Transform = snowmanTransform;
    }

    #region 3D initialisation

    private void InitItemGeometries()
    {
        foreach (var item in _world.Items)
        {
            var geometry = item switch
            {
                Cube => _shapesFactory.CreateCube(GetMaterial(0)),
                Sphere => _shapesFactory.CreateSphere(GetMaterial(1)),
                Beam beam => _shapesFactory.CreateBeam(beam.XSize, beam.YSize, beam.ZSize, GetMaterial(2)),
                Cylinder cyl => _shapesFactory.CreateCylinder(cyl.Radius, cyl.Axis, GetMaterial(3)),
                Cone cone => _shapesFactory.CreateCone(cone.Radius, cone.Axis, GetMaterial(4)),
                // add rectangles with backface culling (no backMaterials parameter used)
                Parallelogram rect => _shapesFactory.CreateParallelogram(rect.Side1, rect.Side2, GetMaterial(5)),
                // show circles without backface culling (by providing a backMaterials parameter).
                Circle circle => _shapesFactory.CreateCircle(normal: circle.Normal, materials: GetMaterial(6), backMaterials: GetMaterial(7)),
                _ => throw new ArgumentException("Unknown type of a item"),
            };
            _itemsList.Add(geometry);
            _itemsGroup.Children.Add(geometry);
        }
        _model3dGroup.Children.Add(_itemsGroup);
        CreateSnowman();
        _model3dGroup.Children.Add(_snowmanGroup);
    }

    private void CreateSnowman()
    {
        var bodyMesh = new MeshGeometry3D();
        _shapesFactory.AddSphereToMesh(bodyMesh, new(0, 50, 0), 100);
        _shapesFactory.AddSphereToMesh(bodyMesh, new(0, 180, 0), 50);
        _snowmanGroup.Children.Add(new GeometryModel3D(bodyMesh, GetMaterial(Colors.Lavender)));

        var hatMesh = new MeshGeometry3D();
        _shapesFactory.AddCylinderToMesh(hatMesh, new(0, 0, 0), 40, new(0, 10, 0));
        _shapesFactory.AddCylinderToMesh(hatMesh, new(0, 10, 0), 30, new(0, 30, 0));
        var hat = new GeometryModel3D(hatMesh, GetMaterial(Colors.LightSlateGray));
        var hatTransform = new Transform3DGroup();
        hatTransform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 20)));
        hatTransform.Children.Add(new TranslateTransform3D(new Vector3D(0, 220, -10)));
        hat.Transform = hatTransform;
        _snowmanGroup.Children.Add(hat);

        var nose = _shapesFactory.CreateCone(4, new(0, 0, 30), GetMaterial(Colors.Orange));
        nose.Transform = new TranslateTransform3D(new(0, 190, 50));
        _snowmanGroup.Children.Add(nose);

        var coalMesh = new MeshGeometry3D();
        _shapesFactory.AddSphereToMesh(coalMesh, new(-20, 200, 40), 5);
        _shapesFactory.AddSphereToMesh(coalMesh, new(20, 200, 40), 5);
        _shapesFactory.AddSphereToMesh(coalMesh, new(0, 110, 80), 3);
        _shapesFactory.AddSphereToMesh(coalMesh, new(0, 90, 92), 3);
        _shapesFactory.AddSphereToMesh(coalMesh, new(0, 70, 98), 3);
        _snowmanGroup.Children.Add(new GeometryModel3D(coalMesh, GetMaterial(Colors.Black)));
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
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_shapesFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }

    private MaterialGroup GetMaterial(int index)
    {
        var color = _colorList[index];
        return GetMaterial(color);
    }

    private static MaterialGroup GetMaterial(Color color)
    {
        var brush = new SolidColorBrush(color);
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
