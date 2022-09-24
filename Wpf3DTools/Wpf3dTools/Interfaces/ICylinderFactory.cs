using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ICylinderFactory
{
    GeometryModel3D Create(Point3D origin, double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true);
    void AddCylinderToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true);
    void AddPipeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Vector3D axis, bool smoothSides = true);
}
