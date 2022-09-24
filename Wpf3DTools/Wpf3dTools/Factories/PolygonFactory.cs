using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class PolygonFactory : IPolygonFactory
{

    public GeometryModel3D CreateParallelogram(Vector3D side1, Vector3D side2, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddParalellogramToMesh(mesh,new Point3D(), side1, side2, textureCoords);
        var geo = new GeometryModel3D(mesh, materials)
        {
            BackMaterial = backMaterials
        };
        return geo;
    }

    public GeometryModel3D CreateCircle(Vector3D normal, MaterialGroup materials, MaterialGroup? backMaterials = null, int numTheta = 72, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddCircleToMesh(mesh, new Point3D(), 1, normal, numTheta, textureCoords);
        var geo = new GeometryModel3D(mesh, materials)
        {
            BackMaterial = backMaterials
        };
        return geo;
    }

    public GeometryModel3D CreatePolygon(Point3D[] points, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddPolygonToMesh(mesh, points, textureCoords);
        var geo = new GeometryModel3D(mesh, materials)
        {
            BackMaterial = backMaterials
        };
        return geo;
    }

    public GeometryModel3D CreateRegularPolygon(int numSides, Vector3D vx, Vector3D vy, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddRegularPolygonToMesh(mesh, numSides, new Point3D(), vx, vy, textureCoords);
        var geo = new GeometryModel3D(mesh, materials)
        {
            BackMaterial = backMaterials
        };
        return geo;
    }

    // Add a polygon with points stored in an array.
    // Texture coordinates are optional.
    public void AddPolygonToMesh(MeshGeometry3D mesh, Point3D[] points, Point[]? textureCoords = null)
    {
        // Make a point dictionary.
        var pointDict = new Dictionary<Point3D, int>();

        // Get the first two point indices.
        int indexA, indexB, indexC;

        if (textureCoords == null)
            indexA = mesh.PointIndex(points[0].Round(), pointDict);
        else
            indexA = mesh.PointIndex(points[0].Round(), textureCoords[0], pointDict);

        if (textureCoords == null)
            indexC = mesh.PointIndex(points[1].Round(), pointDict);
        else
            indexC = mesh.PointIndex(points[1].Round(), textureCoords[1], pointDict);

        // Make triangles.
        for (int i = 2; i < points.Length; i++)
        {
            indexB = indexC;

            if (textureCoords == null)
                indexC = mesh.PointIndex(points[i].Round(), pointDict);
            else
                indexC = mesh.PointIndex(points[i].Round(), textureCoords[i], pointDict);

            if ((indexA != indexB) &&
                (indexB != indexC) &&
                (indexC != indexA))
            {
                mesh.TriangleIndices.Add(indexA);
                mesh.TriangleIndices.Add(indexB);
                mesh.TriangleIndices.Add(indexC);
            }
        }
    }

    // Add a rectangle from center and sides
    public void AddParalellogramToMesh(MeshGeometry3D mesh, Point3D origin, Vector3D side1, Vector3D side2, Point[]? textureCoords = null)
    {
        var points = new Point3D[4];
        points[0] = origin;
        points[1] = origin + side2;
        points[2] = origin + side1 + side2;
        points[3] = origin + side1;
        mesh.AddPolygon(points, textureCoords);
    }

    // add a cricle from center, radius & normalvector
    public void AddCircleToMesh(MeshGeometry3D mesh, Point3D center, double radius, Vector3D normal, int numTheta = 72, Point[]? textureCoords = null)
    {
        var vx = Vector3D.CrossProduct(normal, new Vector3D(1, 0, 0));
        if (vx == new Vector3D()) vx = Vector3D.CrossProduct(normal, new Vector3D(0, 0, 1));
        vx.Normalize();
        vx *= radius;
        var vy = Vector3D.CrossProduct(normal, new Vector3D(0, 1,0));
        if (vy == new Vector3D()) vy = Vector3D.CrossProduct(normal, new Vector3D(0, 0, 1));
        vy.Normalize();
        vy *= radius;
        AddRegularPolygonToMesh(mesh, numTheta, center, vx, vy, textureCoords);
    }

    // Add a regular polygon with optional texture coordinates.
    public void AddRegularPolygonToMesh(MeshGeometry3D mesh, int numSides, Point3D center, Vector3D vx, Vector3D vy, Point[]? textureCoords = null)
    {
        // Generate the points.
        var points = MakePolygonPoints(numSides, center, vx, vy);
        // Make the polygon.
        mesh.AddPolygon(points, textureCoords);
    }

    // Make points to define a regular polygon.
    private static Point3D[] MakePolygonPoints(int numSides, Point3D center, Vector3D vx, Vector3D vy)
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
}
