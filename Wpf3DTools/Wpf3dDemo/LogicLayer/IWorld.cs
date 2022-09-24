using System.Windows.Media.Media3D;
using Wpf3dDemo.Entities;

namespace Wpf3dDemo.Domain;
public interface IWorld
{
    (Point3D p1, Point3D p2) Bounds { get; }
    Point3D Origin { get; }
    List<IItem3D> Items { get; }
}