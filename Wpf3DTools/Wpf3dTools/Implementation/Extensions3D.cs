using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Wpf3dTools.Implementation;
public static class Extensions3D
{
    public static Point3D Round(this Point3D point, int decimals = 3)
    {
        double x = Math.Round(point.X, decimals);
        double y = Math.Round(point.Y, decimals);
        double z = Math.Round(point.Z, decimals);
        return new Point3D(x, y, z);
    }

    public static Point3D SpherePoint(Point3D center, double r, double theta, double phi)
    {
        double y = r * Math.Cos(phi);
        double h = r * Math.Sin(phi);
        double x = h * Math.Sin(theta);
        double z = h * Math.Cos(theta);
        return center + new Vector3D(x, y, z);
    }

    #region PointSharing

    // If the point is already in the dictionary, return its index in the mesh.
    // If the point isn't in the dictionary, create it in the mesh and add its
    // index to the dictionary.
    public static int PointIndex(this MeshGeometry3D mesh, Point3D point, Dictionary<Point3D, int> pointDict)
    {
        // See if the point already exists.
        if ((pointDict != null) && pointDict.ContainsKey(point))
        {
            // The point is already in the dictionary. Return its index.
            return pointDict[point];
        }
        // Create the point.
        int index = mesh.Positions.Count;
        mesh.Positions.Add(point);
        // Add the point's index to the dictionary.
        if (pointDict != null) pointDict.Add(point, index);
        // Return the index.
        return index;
    }

    // If the point is already in the dictionary, return its index in the mesh.
    // If the point isn't in the dictionary, create it and its texture coordinates
    // in the mesh and add its index to the dictionary.
    public static int PointIndex(this MeshGeometry3D mesh, Point3D point, Point textureCoord, Dictionary<Point3D, int> pointDict)
    {
        // See if the point already exists.
        if (pointDict.ContainsKey(point))
        {
            // The point is already in the dictionary. Return its index.
            return pointDict[point];
        }
        // Create the point.
        int index = mesh.Positions.Count;
        mesh.Positions.Add(point);
        // Add the point's texture coordinates.
        mesh.TextureCoordinates.Add(textureCoord);
        // Add the point's index to the dictionary.
        if (pointDict != null) pointDict.Add(point, index);
        // Return the index.
        return index;
    }

    #endregion PointSharing
}
