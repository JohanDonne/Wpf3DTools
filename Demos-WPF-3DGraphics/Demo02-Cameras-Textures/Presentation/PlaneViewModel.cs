using System.Windows;
using System.Windows.Media.Media3D;

namespace Demo02_Cameras_Textures.Presentation;

internal class PlaneViewModel
{
    private MeshGeometry3D _mesh = new();
    public GeometryModel3D Geometry { get; }

    internal PlaneViewModel(double size, double depth, MaterialGroup material)
    {
        InitMesh(size, depth);
        Geometry = new GeometryModel3D(_mesh, material);
    }

    private void InitMesh(double size, double depth)
    {
        Point3D[] points =
        {
            new Point3D(-size/2,-depth,+size/2),  // 0
            new Point3D(-size/2,-depth,-size/2),  // 1
            new Point3D(+size/2,-depth,-size/2),  // 2
            new Point3D(+size/2,-depth,+size/2),  // 3         
        };

        Point[] texturePoints =
        {
            new Point(0,0),
            new Point(0,1),
            new Point(1,1),
            new Point(1,0)
        };

        foreach (var point in points)
        {
            _mesh.Positions.Add(point);
        }
        foreach (var point in texturePoints)
        {
            _mesh.TextureCoordinates.Add(point);
        }

        var triangles = new (int, int, int)[]
        {
            (0,2,1), (0,3, 2)
        };

        foreach ((int i1, int i2, int i3) in triangles)
        {
            _mesh.TriangleIndices.Add(i1);
            _mesh.TriangleIndices.Add(i2);
            _mesh.TriangleIndices.Add(i3);
        }
    }
}
