using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;

public interface ISphericalCameraController
{
    PerspectiveCamera Camera { get; }
    
    void PositionCamera(double radius, double theta, double phi);

    void ControlByKey(Key key);
    void Zoom(int delta);
    void ControlByMouse(Vector vector);

}

