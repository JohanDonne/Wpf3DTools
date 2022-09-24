﻿using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface ISphereFactory
{
    GeometryModel3D CreateSphere(MaterialGroup materials, int numTheta = 72, int numPhi = 72);
    GeometryModel3D CreateSphere(Point3D center, double radius, MaterialGroup materials, int numTheta = 72, int numPhi = 72);
    void AddSphereToMesh(MeshGeometry3D mesh, Point3D center, double radius, int numTheta = 72, int numPhi = 72);
}
