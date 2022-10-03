using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools.Factories;
public class ShapesFactory : IShapesFactory
{
    // derived from original code by Rod Stephens
    // see: Wpf3D, isbn 9781983905964 
    // and https://github.com/WriterRod/WPF-3d-source

    #region Cubes

    public GeometryModel3D CreateCube(MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddCubeToMesh(mesh, new Point3D(), 1.0);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public GeometryModel3D CreateCube(Point3D center, double size, MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddCubeToMesh(mesh, center, size);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public void AddCubeToMesh(MeshGeometry3D mesh, Point3D center, double size)
    {
        Point3D[] points =
                {
                           center + new Vector3D(-size/2,-size/2,+size/2),  //1
                           center + new Vector3D(-size/2,+size/2,+size/2),  //2
                           center + new Vector3D(+size/2,+size/2,+size/2),  //3
                           center + new Vector3D(+size/2,-size/2,+size/2),  //4

                           center + new Vector3D(-size/2,+size/2,-size/2),  //5
                           center + new Vector3D(-size/2,-size/2,-size/2),  //6
                           center + new Vector3D(+size/2,+size/2,-size/2),  //7
                           center + new Vector3D(+size/2,-size/2,-size/2),  //8                           
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

    #endregion Cubes

    #region Spheres

    // create a unity sphere around the origin as a GeormetryModel3D 
    public GeometryModel3D CreateSphere(MaterialGroup materials, int steps = 72)
    {
        var mesh = new MeshGeometry3D();
        AddSphereToMesh(mesh, new Point3D(0, 0, 0), 1, steps);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    public GeometryModel3D CreateSphere(Point3D center, double radius, MaterialGroup materials, int steps = 72)
    {
        var mesh = new MeshGeometry3D();
        AddSphereToMesh(mesh, center, radius, steps);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    // Add a sphere to an existing MeshGeometry3D
    public void AddSphereToMesh(MeshGeometry3D mesh, Point3D center, double radius, int steps = 72)
    {
        // Make a point dictionary if needed.
        var pointDict = new Dictionary<Point3D, int>();

        // Generate the points.
        double dtheta = 2 * Math.PI / steps;
        double dphi = Math.PI / steps;
        double theta = 0;
        for (int t = 0; t < steps; t++)
        {
            double phi = 0;
            for (int p = 0; p < steps; p++)
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

    #endregion Spheres

    #region Beams

    public GeometryModel3D CreateBeam(double xSize, double ySize, double zSize, MaterialGroup materials)
    {
        var mesh = new MeshGeometry3D();
        AddBeamToMesh(mesh, new Point3D(), xSize, ySize, zSize);
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

    #endregion Beams

    #region Cylinders
    public GeometryModel3D CreateCylinder(double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true)
    {
        var mesh = new MeshGeometry3D();
        AddCylinderToMesh(mesh, new(), radius, axis, numSides, smoothSides);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    // Add a cylinder defined by a center point, a radius, and an axis vector.
    // The cylinder will be oriented toward its axis.
    public void AddCylinderToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true)
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
        AddPipeToMesh(mesh, polygon, axis, smoothSides);
    }

    // Add a pipe defined by a center point, a polygon, and an axis vector.
    // The pipe should be oriented toward its axis.
    public void AddPipeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Vector3D axis, bool smoothSides = true)
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

    #endregion cylinders

    #region Cones
    // Create a Cone defined by a centerpoint, radius and axis
    public GeometryModel3D CreateCone(double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true)
    {
        var mesh = new MeshGeometry3D();
        AddConeToMesh(mesh, new Point3D(), radius, axis, numSides, smoothSides);
        var geo = new GeometryModel3D(mesh, materials);
        return geo;
    }

    // Add a Cone defined by a centerpoint, radius and axis
    public void AddConeToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true)
    {
        var apex = origin + axis;

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
        AddConeToMesh(mesh, polygon, apex, smoothSides);
    }

    // Add a cone defined by a polygon and an apex.
    public void AddConeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Point3D apex, bool smoothSides = false)
    {
        // If we should smooth the sides, make the point dictionary.
        Dictionary<Point3D, int>? pointDict = null;
        if (smoothSides) pointDict = new Dictionary<Point3D, int>();

        // Make the sides.
        int numPoints = polygon.Length;
        for (int i = 0; i < polygon.Length; i++)
        {
            int i1 = (i + 1) % numPoints;
            mesh.AddPolygon(pointDict, polygon[i], polygon[i1], apex);
        }

        // Make the bottom.
        var bottom = new Point3D[numPoints];
        Array.Copy(polygon, bottom, numPoints);
        Array.Reverse(bottom);
        mesh.AddPolygon(bottom);
    }

    #endregion Cones

    #region Lines

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

    #endregion Lines

    #region Polygons

    public GeometryModel3D CreateParallelogram(Vector3D side1, Vector3D side2, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddParalellogramToMesh(mesh, new Point3D(), side1, side2, textureCoords);
        var geo = new GeometryModel3D(mesh, materials)
        {
            BackMaterial = backMaterials
        };
        return geo;
    }

    public GeometryModel3D CreateCircle(Vector3D normal, MaterialGroup materials, MaterialGroup? backMaterials = null, int steps = 72, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddCircleToMesh(mesh, new Point3D(), 1, normal, steps, textureCoords);
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

    public GeometryModel3D CreateRegularPolygon(int numSides, Vector3D normal, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)
    {
        var mesh = new MeshGeometry3D();
        AddRegularPolygonToMesh(mesh, numSides, new Point3D(),1.0, normal, textureCoords);
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
        AddRegularPolygonToMesh(mesh, numTheta, center, radius, normal, textureCoords);
    }

    // Add a regular polygon with optional texture coordinates.
    public void AddRegularPolygonToMesh(MeshGeometry3D mesh, int numSides, Point3D center, double size,  Vector3D normal, Point[]? textureCoords = null)
    {
        var vx = Vector3D.CrossProduct(normal, new Vector3D(1, 0, 0));
        if (vx == new Vector3D()) vx = Vector3D.CrossProduct(normal, new Vector3D(0, 0, 1));
        vx.Normalize();
        vx *= size;
        var vy = Vector3D.CrossProduct(normal, new Vector3D(0, 1, 0));
        if (vy == new Vector3D()) vy = Vector3D.CrossProduct(normal, new Vector3D(0, 0, 1));
        vy.Normalize();
        vy *= size;
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

    #endregion Polygons
}
