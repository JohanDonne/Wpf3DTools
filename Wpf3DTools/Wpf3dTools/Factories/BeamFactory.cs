using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class BeamFactory : IBeamFactory
{
    public GeometryModel3D Create(Point3D origin, double xSize, double ySize, double zSize, MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddBeamToMesh(mesh, origin, xSize, ySize, zSize);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public void AddBeamToMesh(MeshGeometry3D mesh, Point3D origin, double xSize, double ySize, double zSize)
    {
        Point3D[] points =
                {
                           origin,                                      //1
                           origin + new Vector3D(0,+ySize,+0),          //2
                           origin + new Vector3D(+xSize,+ySize, 0),     //3
                           origin + new Vector3D(+xSize, 0, 0),         //4

                           origin + new Vector3D(0, +ySize, -zSize),    //5
                           origin + new Vector3D(0,0,-zSize),           //6
                           origin + new Vector3D(+xSize,+ySize,-zSize), //7
                           origin + new Vector3D(+xSize,0,-zSize),      //8                           
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
