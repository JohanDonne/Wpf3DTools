### SphericalCameraController

The `SphericalCameraController` class contains a `PerspectiveCamera` property to be used with a `Viewport3d`control.
The camera is always aimed at the origin of youd 3D scene and can be moved around this origin (as if it was orbiting on a sphere around that origin).    
The Controller provides a set of methods and properties to control the position of the camera using keys (up, down, left, right, +, -) or the mouse.

Namespace:  Wpf3dTools.Implementation

##### Constructors

`SphericalCameraController()`

&nbsp;&nbsp;&nbsp;&nbsp;Creates a camera with a 60° field of view at a distance of 30 units from the origin.

##### Properties

`Camera`    

The camera instance to be bound to the `Viewport3D` that will show the 3D scene (PerspectiveCamera, readonly).    


##### Methods

`void PositionCamera(double radius, double theta, double phi)`

Positions the camera at the passed coördinates (always looking at the origin).   
 
`void ControlByKey(Key key)`    

Moves the camera according to the Key value passed (up, down, left, right, +, -) maintaining a constant distance from the origin.   

`void ControlByMouse(Vector vector)`    

Moves the camera according to the vector passed (representing a mouse movement) maintaining a constant distance from the origin.    

`void Zoom(int delta)`   

Moves the camera closer to the origin (for positive values of delta) or farther away from it (for negative values of delta).   



##### Events 

None



