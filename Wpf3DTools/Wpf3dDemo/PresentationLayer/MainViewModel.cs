using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dDemo.Domain;
using Wpf3dDemo.LogicLayer;
using Wpf3dTools.Interfaces;

namespace Wpf3dDemo.PresentationLayer;
public class MainViewModel
{
	private readonly IWorld _world;
    private readonly ISphericalCameraController _cameraController;
    private readonly ILineFactory _lineFactory;
    private readonly Model3DGroup _model3dGroup = new();
    private Model3DGroup? _axesGroup;
    private bool _showAxes;

    public ProjectionCamera Camera => _cameraController.Camera;
    public Model3D Visual3dContent => _model3dGroup;

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

    public MainViewModel(IWorld world, ISphericalCameraController cameraController, ILineFactory lineFactory)
    {
        _world = world;
        _cameraController = cameraController;
        _lineFactory = lineFactory;
        Init3DPresentation();
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
        _axesGroup = new Model3DGroup();
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(xLength, 0, 0), new Point3D(0, 0, 0), thickness, Brushes.Red));
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(0, yLength, 0), new Point3D(0, 0, 0), thickness, Brushes.Green));
        _axesGroup.Children.Add(_lineFactory.CreateLine(new Point3D(0, 0, zLength), new Point3D(0, 0, 0), thickness, Brushes.Blue));
        _axesGroup.Freeze();
    }

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
