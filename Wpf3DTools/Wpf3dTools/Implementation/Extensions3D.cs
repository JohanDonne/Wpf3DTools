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

    #region Polygon

    // Make points to define a regular polygon.
    public static Point3D[] MakePolygonPoints(int numSides,
        Point3D center, Vector3D vx, Vector3D vy)
    {
        // Generate the points.
        var points = new Point3D[numSides];
        double dtheta = 2 * Math.PI / numSides;
        double theta = Math.PI / 2;
        for (int i = 0; i < numSides; i++)
        {
            points[i] = center + (vx * Math.Cos(theta)) + (vy * Math.Sin(theta));
            theta += dtheta;
        }
        return points;
    }

    // Add a polygon with points stored in an array.
    // Texture coordinates are optional.
    public static void AddPolygon(this MeshGeometry3D mesh,
        Point3D[] points, Point[]? textureCoords = null)
    {
        mesh.AddPolygon(points, null, textureCoords);
    }
    public static void AddPolygon(this MeshGeometry3D mesh,
        Point3D[] points, Dictionary<Point3D, int>? pointDict = null,
        Point[]? textureCoords = null)
    {
        pointDict ??= new Dictionary<Point3D, int>();

        // Get the first two point indices.
        int indexA, indexB, indexC;

        var roundedA = points[0].Round();
        indexA = textureCoords == null ? mesh.PointIndex(roundedA, pointDict) : mesh.PointIndex(roundedA, textureCoords[0], pointDict);

        var roundedC = points[1].Round();
        indexC = textureCoords == null ? mesh.PointIndex(roundedC, pointDict) : mesh.PointIndex(roundedC, textureCoords[1], pointDict);

        // Make triangles.
        Point3D roundedB;
        for (int i = 2; i < points.Length; i++)
        {
            indexB = indexC;
            roundedB = roundedC;

            // Get the next point.
            roundedC = points[i].Round();
            indexC = textureCoords == null
                ? mesh.PointIndex(points[i].Round(), pointDict)
                : mesh.PointIndex(points[i].Round(), textureCoords[i], pointDict);

            // If two of the points are the same, skip this triangle.
            if ((roundedA != roundedB) &&
                (roundedB != roundedC) &&
                (roundedC != roundedA))
            {
                mesh.TriangleIndices.Add(indexA);
                mesh.TriangleIndices.Add(indexB);
                mesh.TriangleIndices.Add(indexC);
            }
        }
    }

    // Add a polygon with a variable argument list of points
    // and no texture coordinates.
    public static void AddPolygon(this MeshGeometry3D mesh,
        Dictionary<Point3D, int>? pointDict = null,
        params Point3D[] points)
    {
        mesh.AddPolygon(points, pointDict, null);
    }
    public static void AddPolygon(this MeshGeometry3D mesh,
        params Point3D[] points)
    {
        mesh.AddPolygon(points, null);
    }

    // Add a regular polygon with optional texture coordinates.
    public static void AddRegularPolygon(this MeshGeometry3D mesh,
        int numSides, Point3D center, Vector3D vx, Vector3D vy,
        Point[]? textureCoords = null)
    {
        // Generate the points.
        var points = MakePolygonPoints(numSides, center, vx, vy);

        // Make the polygon.
        mesh.AddPolygon(points, textureCoords);
    }

    #endregion Polygon

}
