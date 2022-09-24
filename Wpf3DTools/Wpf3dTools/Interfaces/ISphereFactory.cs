using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ISphereFactory
{
    GeometryModel3D Create(MaterialGroup materials, int numTheta = 72, int numPhi = 72);
    GeometryModel3D Create(Point3D center, double radius, MaterialGroup materials, int numTheta = 72, int numPhi = 72);
}
