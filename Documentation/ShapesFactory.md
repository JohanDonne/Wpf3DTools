## ShapesFactory

The `ShapesFactory` class provides methods for creating `GeometryModel3D` instances for basic geometric shapes (Cubes, beams, Spheres, Cylinders, Cones, polygons, lines) and for extending an existing `MeshGeometry3D` with such shapes.

Namespace:  Wpf3dTools.Factories

##### Constructors

`ShapesFactory()`

&nbsp;&nbsp;&nbsp;&nbsp;Creates a `ShapesFactory` instance on which the desired methods can be called.

##### Properties

None 


##### Methods

`GeometryModel3D CreateCube(MaterialGroup materials)`

Returns a unity cube centered around the origin aligned with the X-, Y- and Z-axes.   

`GeometryModel3D CreateCube(Point3D center, double size, MaterialGroup materials)`

Returns a cube with the specified size and center, aligned with the X-, Y- and Z-axes.

`void AddCubeToMesh(MeshGeometry3D mesh, Point3D center, double size)`

Extends the mesh that is passed in with a cube at the specified center-position, with the specified size and aligned with the X-, Y- and Z-axes. 
    
`GeometryModel3D CreateSphere(MaterialGroup materials, int steps = 72)`

Returns a unity sphere centered around the origin. Optionally, a number of steps for the angles in the horizontal and vertical planes can be specified (the more steps, the smoother the sphere will be, but at a cost of a larger mesh).

`GeometryModel3D CreateSphere(Point3D center, double radius, MaterialGroup materials, int steps = 72)`

Returns a sphere at the specified position with the specified radius. Optionally, a number of steps for the angles in the horizontal and vertical planes can be specified (the more steps, the smoother the sphere will be, but at a cost of a larger mesh).

`void AddSphereToMesh(MeshGeometry3D mesh, Point3D center, double radius, int steps = 72)`

Extends the mesh that is passed in with a sphere at the specified center-position, with the specified radius. Optionally, a number of steps for the angles in the horizontal and vertical planes can be specified (the more steps, the smoother the sphere will be, but at a cost of a larger mesh).

`GeometryModel3D CreateBeam(double xSize, double ySize, double zSize, MaterialGroup materials)`

Returns a beam originating in the origin, with the specified sizes, aligned with the X-, Y- and Z-axes.

`void AddBeamToMesh(MeshGeometry3D mesh, Point3D origin, double xSize, double ySize, double zSize)`

Extends the mesh that is passed in with a beam at the specified origin and with the specified sizes, aligned with the X-, Y- and Z-axes.

`GeometryModel3D CreateCylinder(double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true)`

Returns a cylinder, centered around the origin, with the length and direction specified by `axis`. Optionally, a number of sides for the cylinder approximation can be specified (the more sides, the smoother the cylinder will be, but at a cost of a larger mesh).

`void AddCylinderToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true)`

Extends the mesh that is passed in with a cylinder at the specified origin, with the length and direction specified by `axis`. Optionally, a number of sides for the cylinder approximation can be specified (the more sides, the smoother the sphere will be, but at a cost of a larger mesh).

`void AddPipeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Vector3D axis, bool smoothSides = true)`

Extends the mesh that is passed in with a cylinder based on the polygon (it should be convex), with the length and direction specified by `axis`. The groundplane of the pipe is defined by the polygon that is passed.

`GeometryModel3D CreateCone(double radius, Vector3D axis, MaterialGroup materials, int numSides = 72, bool smoothSides = true)`

Returns a cone, centered around the origin, with the apex specified by `axis`. Optionally, a number of sides for the cone approximation can be specified (the more sides, the smoother the cone will be, but at a cost of a larger mesh).

`void AddConeToMesh(MeshGeometry3D mesh, Point3D origin, double radius, Vector3D axis, int numSides = 72, bool smoothSides = true)`

Extends the mesh that is passed in with a cone centered around the specified origin, with the apex specified by `axis`. Optionally, a number of sides for the cone approximation can be specified (the more sides, the smoother the cone will be, but at a cost of a larger mesh).

`void AddConeToMesh(MeshGeometry3D mesh, Point3D[] polygon, Point3D apex, bool smoothSides = false)`

Extends the mesh that is passed in with a cone based on the specified polygon (it should be convex), with the apex specified by `axis`. Optionally, a number of sides for the cone approximation can be specified (the more sides, the smoother the cone will be, but at a cost of a larger mesh).

`GeometryModel3D CreateLine(Point3D start, Point3D end, double thickness, Brush brush)`

Returns a line between the specified points.

`void AddLine(MeshGeometry3D mesh, Point3D start, Point3D end, double thickness)`

Extends the mesh that is passed in with a line between the specified points.

`GeometryModel3D CreateParallelogram(Vector3D side1, Vector3D side2, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)`

Returns a parallogram defined by the origin and the two sides. Optionally, a `backMaterial`can be used (otherwise backface culling will be applied, making the parallelogram only visible form the 'front' side). Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the parallelogram.

`GeometryModel3D CreateCircle(Vector3D normal, MaterialGroup materials, MaterialGroup? backMaterials = null, int steps = 72, Point[]? textureCoords = null)`

Returns a unity circle around the origin oriented to the provided normal vector. Optionally, a `backMaterial`can be used (otherwise backface culling will be applied, making the circle only visible form the 'front' side). Optionally a number of steps for the circle approximation can be specified (the more steps, the smoother the circle will be, but at a cost of a larger mesh). Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the circle.

`GeometryModel3D CreateRegularPolygon(int numSides, Vector3D vx, Vector3D vy, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)`

Returns a regular polygon around the origin, with all points a distance of 1 unit from the origin and oriented to the provided normal vector. Optionally, a `backMaterial`can be used (otherwise backface culling will be applied, making the circle only visible form the 'front' side). Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the polygon.

`GeometryModel3D CreatePolygon(Point3D[] points, MaterialGroup materials, MaterialGroup? backMaterials = null, Point[]? textureCoords = null)`

Returns a polygon defined by the passed point-array (should be convex).  Optionally, a `backMaterial`can be used (otherwise backface culling will be applied, making the parallelogram only visible form the 'front' side). Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the parallelogram.

`void AddParalellogramToMesh(MeshGeometry3D mesh, Point3D origin, Vector3D side1, Vector3D side2, Point[]? textureCoords = null)`

Extends the mesh that is passed in with a Parallelogram defined by the passed origin and the two sides. Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the parallelogram.

`void AddCircleToMesh(MeshGeometry3D mesh, Point3D center, double radius, Vector3D normal, int steps = 72, Point[]? textureCoords = null);`

Extends the mesh that is passed in with a circle around the center, oriented to the provided normal vector. Optionally, a number of steps for the circle approximation can be specified (the more steps, the smoother the circle will be, but at a cost of a larger mesh). Optionally, if the material that is passed contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the circle.

`void AddRegularPolygonToMesh(MeshGeometry3D mesh, int numSides, Point3D center, double size, Vector3D normal, Point[]? textureCoords = null);`

Extends the mesh that is passed in with a regular polygon around the provided center, with all points a distance of `size` from the center and oriented to the provided normal vector. Optionally, if the material that will be used contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the polygon.

`void AddPolygonToMesh(MeshGeometry3D mesh, Point3D[] points, Point[]? textureCoords = null);`

Extends the mesh that is passed in with a polygon defined by the passed point-array. These points should form a convex polygon. Optionally, if the material that will be used contains a texture/image, the appropriate texture coördinates can be passed to be used in mapping the texture/image to the polygon.
