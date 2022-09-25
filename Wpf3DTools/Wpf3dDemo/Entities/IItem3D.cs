using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Wpf3dDemo.Entities;
public interface IItem3D
{
    public Point3D  Position { get; }

    public double Scale { get;  }

    public double YRotation { get; }
}
