using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ICubeFactory
{
    GeometryModel3D CreateCube(MaterialGroup materials);
    GeometryModel3D CreateCube(Point3D center, double size, MaterialGroup materials);
    void AddCubeToMesh(MeshGeometry3D mesh, Point3D center, double size);

}