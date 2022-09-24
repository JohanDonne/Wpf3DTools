using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface IConeFactory
{
    GeometryModel3D CreateCone(double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true);
    void AddConeToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = false);
    void AddConeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Point3D apex, bool smoothSides = false);
}