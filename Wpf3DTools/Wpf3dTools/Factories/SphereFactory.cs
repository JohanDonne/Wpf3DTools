using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class SphereFactory : ISphereFactory
{
    // create a unity sphere around the origin as a GeormetryModel3D 
    public GeometryModel3D Create(MaterialGroup materials, int numTheta = 72, int numPhi = 72)
    {
        var mesh = new MeshGeometry3D();
        AddSphereToMesh(mesh, new Point3D(0, 0, 0), 1, numTheta, numPhi);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public GeometryModel3D Create(Point3D center, double radius, MaterialGroup materials, int numTheta = 72, int numPhi = 72)
    {
        var mesh = new MeshGeometry3D();
        AddSphereToMesh(mesh, center, radius, numTheta, numPhi);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    // Add a sphere to an existing MeshGeometry3D
    public void AddSphereToMesh(MeshGeometry3D mesh, Point3D center, double radius, int numTheta = 72, int numPhi = 72)
    {
        // Make a point dictionary if needed.
        var pointDict = new Dictionary<Point3D, int>();

        // Generate the points.
        double dtheta = 2 * Math.PI / numTheta;
        double dphi = Math.PI / numPhi;
        double theta = 0;
        for (int t = 0; t < numTheta; t++)
        {
            double phi = 0;
            for (int p = 0; p < numPhi; p++)
            {
                // Find this piece's points.
                Point3D[] points =
                {
                        SpherePoint(center, radius, theta, phi),
                        SpherePoint(center, radius, theta, phi + dphi),
                        SpherePoint(center, radius, theta + dtheta, phi + dphi),
                        SpherePoint(center, radius, theta + dtheta, phi),
                    };
                // Make the polygon.
                mesh.AddPolygon(pointDict, points);
                phi += dphi;
            }
            theta += dtheta;
        }
    }

    private static Point3D SpherePoint(Point3D center, double r, double theta, double phi)
    {
        double y = r * Math.Cos(phi);
        double h = r * Math.Sin(phi);
        double x = h * Math.Sin(theta);
        double z = h * Math.Cos(theta);
        return center + new Vector3D(x, y, z);
    }
}
