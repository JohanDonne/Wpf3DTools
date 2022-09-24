﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record struct Cube : IItem3D
{
    public Point3D Position { get ; set ; }
    public double Scale => Size;
    public double Size { get ; set ; }  
}
