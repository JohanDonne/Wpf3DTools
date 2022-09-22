using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ILineFactory
{
    void AddLine(MeshGeometry3D mesh, Point3D start, Point3D end, double thickness);
    GeometryModel3D CreateLine(Point3D start, Point3D end, double thickness, Brush brush);
}