﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record struct Cone: IItem3D
{
    public Point3D Position { get; set; }

    public double Scale { get; set; }

    public double Radius { get; }

    public Vector3D Axis { get; }

    public Cone(Point3D position, double radius, Vector3D axis, double scale = 1)
    {
        Position = position;
        Radius = radius;
        Axis = axis;
        Scale = scale;
    }
}
