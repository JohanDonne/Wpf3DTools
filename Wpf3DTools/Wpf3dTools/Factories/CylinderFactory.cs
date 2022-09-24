using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class CylinderFactory : ICylinderFactory
{

    public GeometryModel3D Create(Point3D origin, double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true)
    {
        var mesh = new MeshGeometry3D();
        AddCylinderToMesh(mesh, origin, radius, axis, numSides, smoothSides);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    // Add a cylinder defined by a center point, a radius, and an axis vector.
    // The cylinder should be oriented toward its axis.
    public static void AddCylinderToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true)
    {
        // calculate bottom polygon (as circle)
        var polygon = new Point3D[numSides];

        // prepare matrix for rotating radius vector around axis
        double angleDelta = 360.0 / numSides;
        var rotationMatrix = Matrix3D.Identity;
        var q = new Quaternion(axis, angleDelta);
        rotationMatrix.Rotate(q);

        // determine startvalue vor radius vector
        var radiusVector = Vector3D.CrossProduct(axis, new Vector3D(0, 0, 1));
        if (radiusVector == new Vector3D()) radiusVector = Vector3D.CrossProduct(axis, new Vector3D(0, 1, 0));
        radiusVector.Normalize();
        radiusVector *= radius;
        // and calculate points on Circle;
        for (int index = 0; index < numSides; index++)
        {
            polygon[index] = origin + radiusVector;
            radiusVector = rotationMatrix.Transform(radiusVector);
        }
        AddPipe(mesh, polygon, axis, smoothSides);
    }

    // Add a pipe defined by a center point, a polygon, and an axis vector.
    // The pipe should be oriented toward its axis.
    public static void AddPipe(MeshGeometry3D mesh,
        Point3D[] polygon, Vector3D axis, bool smoothSides = true)
    {
        // If we should smooth the sides, make the point dictionary.
        Dictionary<Point3D, int>? pointDict = null;
        if (smoothSides) pointDict = new Dictionary<Point3D, int>();

        // Make the top.
        int numPoints = polygon.Length;
        var top = new Point3D[numPoints];
        for (int i = 0; i < polygon.Length; i++)
        {
            top[i] = polygon[i] + axis;
        }
        mesh.AddPolygon(top);

        // Make the sides.
        for (int i = 0; i < polygon.Length; i++)
        {
            int i1 = (i + 1) % numPoints;
            mesh.AddPolygon(pointDict, polygon[i], polygon[i1], top[i1], top[i]);
        }

        // Make the bottom.
        var bottom = new Point3D[numPoints];
        Array.Copy(polygon, bottom, numPoints);
        Array.Reverse(bottom);
        mesh.AddPolygon(bottom);
    }
}
