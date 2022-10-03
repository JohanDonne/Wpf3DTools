### Integrating Wpf3dTools with Microsoft Dependency Injection

The Wpf3dTools library can be easily integrated with .Net Core Dependency Injection.

* Interfaces have been declared for both public classes in the library:    
  * `ISphericalCameraController`
  * `IShapesFactory`   
    which comprises the following subordinate interfaces: `ICubeFactory`, `ISphereFactory`, `IBeamFactory`, `ICylinderFactory`, `IConeFactory`, `ILineFactory`, `IPolygonFactory`

* An Extension method is provided to register the relevant services: `AddWpf3dServices()`.

The sample application in the GitHub repository uses DI and the MVVM architectural pattern.