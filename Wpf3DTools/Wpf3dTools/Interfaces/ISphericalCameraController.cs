using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;

public interface ISphericalCameraController
{
    PerspectiveCamera Camera { get; }
    Point3D CartesianCoordinates { get; set; }
    Point3D SphericalCoordinates { get; set; }

    void DecreasePhi();
    void DecreasePhi(double amount);
    void DecreaseR();
    void DecreaseR(double amount);
    void DecreaseTheta();
    void DecreaseTheta(double amount);
    void IncreasePhi();
    void IncreasePhi(double amount);
    void IncreaseR();
    void IncreaseR(double amount);
    void IncreaseTheta();
    void IncreaseTheta(double amount);

    void PositionCamera(double radius, double theta, double phi);

    void ControlByKey(Key key);
    void Zoom(int delta);
    void ControlByMouse(Vector vector);

}

