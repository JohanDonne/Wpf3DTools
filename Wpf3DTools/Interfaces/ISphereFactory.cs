using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ISphereFactory
{
    GeometryModel3D CreateSphere(MaterialGroup materials, int steps = 72);
    GeometryModel3D CreateSphere(Point3D center, double radius, MaterialGroup materials, int steps = 72);
    void AddSphereToMesh(MeshGeometry3D mesh, Point3D center, double radius, int steps = 72);
}
