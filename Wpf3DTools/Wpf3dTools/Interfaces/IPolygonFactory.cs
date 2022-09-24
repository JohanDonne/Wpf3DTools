using System.Windows;
using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface IPolygonFactory
{
    void AddPolygonToMesh(MeshGeometry3D mesh, Point3D[] points, Point[]? textureCoords = null);
    void AddParalellogramToMesh(MeshGeometry3D mesh, Point3D origin, Vector3D side1, Vector3D side2, Point[]? textureCoords = null);
    void AddRegularPolygonToMesh(MeshGeometry3D mesh, int numSides, Point3D center, Vector3D vx, Vector3D vy, Point[]? textureCoords = null);
    void AddCircleToMesh(MeshGeometry3D mesh, Point3D center, double radius, Vector3D normal, int numTheta = 72, Point[]? textureCoords = null);
    GeometryModel3D CreatePolygon(Point3D[] points, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null);
    GeometryModel3D CreateParallelogram(Vector3D side1, Vector3D side2, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null);
    GeometryModel3D CreateRegularPolygon(int numSides, Vector3D vx, Vector3D vy, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null);
    GeometryModel3D CreateCircle(Vector3D normal, MaterialGroup materials, MaterialGroup? backMaterials = null, int numTheta = 72, Point[]? textureCoords = null);
}