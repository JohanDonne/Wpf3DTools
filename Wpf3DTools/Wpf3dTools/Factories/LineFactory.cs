using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class LineFactory : ILineFactory
{
    public GeometryModel3D CreateLine(Point3D start, Point3D end, double thickness, Brush brush)
    {
        var mesh = new MeshGeometry3D();
        AddLine(mesh, start, end, thickness);
        var material = new DiffuseMaterial(brush);
        // Create the model, which includes the geometry and material.
        var geo = new GeometryModel3D(mesh, material);
        // geo.Freeze();
        return geo;
    }

    // Add a 3D-line to an existing MeshGeometry3D
    public void AddLine(MeshGeometry3D mesh, Point3D start, Point3D end, double thickness)
    {
        var direction = end - start;
        Vector3D v1;
        if (Math.Abs(direction.Z) > 0.00001)
        {
            v1 = new Vector3D(1, 1, -1.0 * (direction.X + direction.Y) / direction.Z);
        }
        else if (Math.Abs(direction.X) > 0.00001)
        {
            v1 = new Vector3D(-1.0 * (direction.Y + direction.Z) / direction.X, 1, 1);
        }
        else if (Math.Abs(direction.Y) > 0.00001)
        {
            v1 = new Vector3D(1, -1.0 * (direction.X + direction.Z) / direction.Y, 1);
        }
        else
        {
            throw new ArgumentException("Line Length must not be zero");
        }

        var v2 = Vector3D.CrossProduct(direction, v1);
        v1.Normalize();
        v2.Normalize();
        v1 *= thickness / 2;
        v2 *= thickness / 2;
        var points = new Point3D[]
            { (start - v1).Round(),
                  (start + v1).Round(),
                  (start - v2).Round(),
                  (start + v2).Round(),
                  (start - v1 + direction).Round(),
                  (start + v1 + direction).Round(),
                  (start - v2 + direction).Round(),
                  (start + v2 + direction).Round()
            };
        var pointDict = new Dictionary<Point3D, int>();
        int[] indices = points.Select(p => mesh.PointIndex(p, pointDict)).ToArray();
        var triangles = new (int, int, int)[]
        {
              (0,1,2), (0,3,1),
              (3,7,1), (1,7,5),
              (1,5,2), (2,5,6),
              (0,2,4), (4,2,6),
              (0,4,3), (3,4,7),
              (4,6,5),(4,5,7)
        };
        foreach ((int i1, int i2, int i3) in triangles)
        {
            mesh.TriangleIndices.Add(indices[i1]);
            mesh.TriangleIndices.Add(indices[i2]);
            mesh.TriangleIndices.Add(indices[i3]);
        }
    }
}
