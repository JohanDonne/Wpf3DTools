using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class CubeFactory : ICubeFactory
{
    public GeometryModel3D Create(MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddCubeToMesh(mesh, new Point3D(), 1.0, 1.0, 1.0);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public GeometryModel3D Create(Point3D center, double xSize, double ySize, double zSize, MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddCubeToMesh(mesh, center, xSize, ySize, zSize);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public static void AddCubeToMesh(MeshGeometry3D mesh, Point3D center, double xSize, double ySize, double zSize)
    {
        Point3D[] points =
                {
                           center + new Vector3D(-xSize/2,-ySize/2,+zSize/2),  //1
                           center + new Vector3D(-xSize/2,+ySize/2,+zSize/2),  //2
                           center + new Vector3D(+xSize/2,+ySize/2,+zSize/2),  //3
                           center + new Vector3D(+xSize/2,-ySize/2,+zSize/2),  //4

                           center + new Vector3D(-xSize/2,+ySize/2,-zSize/2),  //5
                           center + new Vector3D(-xSize/2,-ySize/2,-zSize/2),  //6
                           center + new Vector3D(+xSize/2,+ySize/2,-zSize/2),  //7
                           center + new Vector3D(+xSize/2,-ySize/2,-zSize/2),  //8                           
                    };
        var triangles = new (int, int, int)[]
        {
              (0,2,1), (0,3,2),
              (0,1,4), (0,4,5),
              (0,5,3), (3,5,7),
              (4,6,5), (5,6,7),
              (2,3,7), (7,6,2),
              (1,2,4), (2,6,4)
        };
        var pointDict = new Dictionary<Point3D, int>();
        int[] indices = points.Select(p => mesh.PointIndex(p, pointDict)).ToArray();
        foreach ((int i1, int i2, int i3) in triangles)
        {
            mesh.TriangleIndices.Add(indices[i1]);
            mesh.TriangleIndices.Add(indices[i2]);
            mesh.TriangleIndices.Add(indices[i3]);
        }
    }
}
