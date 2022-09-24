using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface IBeamFactory
{
    GeometryModel3D Create(Point3D origin, double xSize, double ySize, double zSize, MaterialGroup materials);
}
