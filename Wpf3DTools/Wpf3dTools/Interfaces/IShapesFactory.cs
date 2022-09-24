using System.Windows.Media.Media3D;

namespace Wpf3dTools.Interfaces;
public interface IShapesFactory: ICubeFactory, ISphereFactory, IBeamFactory, ICylinderFactory, IConeFactory, ILineFactory, IPolygonFactory
{
}