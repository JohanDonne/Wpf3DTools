using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Wpf3DUtils
{
    public static class Models3D
    {
        // method to ensure point-coördinates are rounded to 3 decimals to make them usable as dictionary-keys
        public static Point3D Round(this Point3D point, int decimals = 3)
        {
            double x = Math.Round(point.X, decimals);
            double y = Math.Round(point.Y, decimals);
            double z = Math.Round(point.Z, decimals);
            return new Point3D(x, y, z);
        }

        // Return a point on a sphere.
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
        public static int PointIndex(this MeshGeometry3D mesh,
            Point3D point, Dictionary<Point3D, int>? pointDict = null)
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
        public static int PointIndex(this MeshGeometry3D mesh,
            Point3D point, Point textureCoord,
            Dictionary<Point3D, int>? pointDict = null)
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
            // Make a point dictionary.
            if (pointDict == null) pointDict = new Dictionary<Point3D, int>();

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

        #region Models

        // create a new standalone 3D-line as a GeormetryModel3D
        public static GeometryModel3D CreateLine(Point3D start, Point3D end, double thickness, Brush brush)
        {
            var mesh = new MeshGeometry3D();
            mesh.AddLine(start, end, thickness);
           
            var material = new DiffuseMaterial(brush);
            // Create the model, which includes the geometry and material.
            var geo = new GeometryModel3D(mesh, material);
            return geo;
        }

        // Add a 3D-line to an existing MeshGeometry3D
        public static void AddLine(this MeshGeometry3D mesh, Point3D start, Point3D end, double thickness)
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

        // create a unity sphere around the origin as a GeormetryModel3D 
        public static GeometryModel3D CreateSphere(MaterialGroup materials, int numTheta = 36, int numPhi = 36)
        {
            var mesh = new MeshGeometry3D();
            mesh.AddSphere(new Point3D(0, 0, 0), 1, numTheta, numPhi);
            var geo = new GeometryModel3D(mesh, materials);
            return geo;
        }

        // Add a sphere to an existing MeshGeometry3D
        public static void AddSphere(this MeshGeometry3D mesh, Point3D center, double radius, int numTheta, int numPhi)
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

        // create a unity cube around the origin as a GeormetryModel3D 
        public static GeometryModel3D CreateCube(MaterialGroup materials)
        {
            var mesh = new MeshGeometry3D();
            mesh.AddCube(new Point3D(), 1.0, 1.0, 1.0);
            var geo = new GeometryModel3D(mesh, materials);
            return geo;
        }

        // Add a cube to an existing MeshGeometry3D
        public static void AddCube(this MeshGeometry3D mesh, Point3D center, double xSize, double ySize, double zSize)
        {
            Point3D[] points =
                    {
                           center + new Vector3D(-xSize/2,-ySize/2,+zSize/2),  
                           center + new Vector3D(-xSize/2,+ySize/2,+zSize/2), 
                           center + new Vector3D(+xSize/2,+ySize/2,+zSize/2), 
                           center + new Vector3D(+xSize/2,-ySize/2,+zSize/2), 

                           center + new Vector3D(-xSize/2,+ySize/2,-zSize/2),
                           center + new Vector3D(-xSize/2,-ySize/2,-zSize/2),
                           center + new Vector3D(+xSize/2,+ySize/2,-zSize/2), 
                           center + new Vector3D(+xSize/2,-ySize/2,-zSize/2),                     
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

        #endregion Models

    }
}
