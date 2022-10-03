# Wpf3DTools

A small library to facilitate working with 3D scenes in WPF.

WPF offers a lot of features aimed at creating attractive applications, including excellent support for 3D-graphics.    
This library may help you to get started writing such 3D-applications.    
It contains: 

- Procedural mesh generators for basic geometric models
- A simple spherical camera controller

Part of this library is derived from the code in the book "WPF 3d, Three-Dimensional Graphics with WPF and C#" by [Rod Stephens](https://github.com/WriterRod) (ISBN-13: 978-1983905964). 

Note: This library targets .Net 6 and (obviously) Windows WPF applications.

Documentation: 

[Getting Started](Documentation/GettingStarted.md)    
[The Spherical Camera Controller](Documentation/SphericalCameraController.md)    
[Creating basig geometrical shapes: The ShapesFactory](Documentation/ShapesFactory)    
[Using Wpf3dTools with DependencyInjection](Documentation/DependencyInjection)    


The sample application contained in this repository demonstrates the use of this library and the animation of shapes by manipulation of their associated transformations.
