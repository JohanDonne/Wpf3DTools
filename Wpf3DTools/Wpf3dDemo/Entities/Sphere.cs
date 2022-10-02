﻿using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record class Sphere : IItem3D
{
    public Point3D Position { get; set; }
    public double Scale => Radius;
    public double YRotation => 0;
    public double Radius { get; init; }
    public Sphere(Point3D position, double radius)
    {
        Position = position;
        Radius = radius;
    }
}
