using System.Windows.Media.Media3D;

namespace Demo02_Cameras_Textures.Presentation;

internal class CubeViewModel
{
    private MeshGeometry3D _mesh = new();
    public GeometryModel3D Geometry { get; }

    internal CubeViewModel(double size, MaterialGroup material)
    {
        InitMesh(size);
        Geometry = new GeometryModel3D(_mesh, material);
    }

    private void InitMesh(double size)
    {
        Point3D[] points =
                    {
                           new Point3D(-size/2,-size/2,+size/2),  // 0
                           new Point3D(-size/2,+size/2,+size/2),  // 1
                           new Point3D(+size/2,+size/2,+size/2),  // 2
                           new Point3D(+size/2,-size/2,+size/2),  // 3

                           new Point3D(-size/2,+size/2,-size/2),  // 4
                           new Point3D(-size/2,-size/2,-size/2),  // 5
                           new Point3D(+size/2,+size/2,-size/2),  // 6
                           new Point3D(+size/2,-size/2,-size/2),  // 7                           
                    };

        foreach (var point in points)
        {
            _mesh.Positions.Add(point);
        }

        var triangles = new (int, int, int)[]
            {
              (0,2,1), (0,3,2),  // front
              (0,1,4), (0,4,5),  // left 
              (0,5,3), (3,5,7),  // bottom
              (4,6,5), (5,6,7),  // back
              (2,3,7), (7,6,2),  // right
              (1,2,4), (2,6,4)   // top
            };

        foreach ((int i1, int i2, int i3) in triangles)
        {
            _mesh.TriangleIndices.Add(i1);
            _mesh.TriangleIndices.Add(i2);
            _mesh.TriangleIndices.Add(i3);
        }
    }
}
