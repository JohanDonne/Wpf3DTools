using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record struct Beam: IItem3D
{
    public Point3D Position { get;set; }
    public double Scale { get; set; } 
    public double XSize { get; }
    public double YSize { get; }
    public double ZSize { get; }

    public Beam(Point3D position, double xSize, double ySize, double zSize, double scale = 1) 
    {
        Position = position;
        XSize = xSize;
        YSize = ySize;
        ZSize = zSize;
        Scale = scale;
    }
}
