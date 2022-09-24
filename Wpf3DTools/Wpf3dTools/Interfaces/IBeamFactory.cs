using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface IBeamFactory
{
    GeometryModel3D Create(double xSize, double ySize, double zSize, MaterialGroup materials);
    void AddBeamToMesh(MeshGeometry3D mesh, Point3D origin, double xSize, double ySize, double zSize);
}
