using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public class Cone: IItem3D
{
    public Point3D Position { get; set; }

    public double Scale => 1;

    public double Radius { get; set; }

    public Vector3D Axis { get; set; }
}
