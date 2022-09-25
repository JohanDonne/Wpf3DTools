using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public record class Cube : IItem3D
{
    public Point3D Position { get; set; }
    public double Scale => Size;
    public double YRotation { get; set; } = 0;
    public double Size { get ;}
    
    public Cube(Point3D position, double size)
    {
        Position = position;
        Size = size;
    }
}
