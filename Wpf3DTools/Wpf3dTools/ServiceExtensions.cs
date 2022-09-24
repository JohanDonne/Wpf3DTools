using Microsoft.Extensions.DependencyInjection;
using Wpf3dTools.Factories;
using Wpf3dTools.Implementation;
using Wpf3dTools.Interfaces;

namespace Wpf3dTools;

public static class ServiceExtensions
{
    public static void AddWpf3dServices(this ServiceCollection services)
    {
        services.AddTransient<ISphericalCameraController, SphericalCameraController>();
        services.AddSingleton<ICubeFactory, CubeFactory>();
        services.AddSingleton<IBeamFactory, BeamFactory>();
        services.AddSingleton<ISphereFactory, SphereFactory>();
        services.AddSingleton<ICylinderFactory, CylinderFactory>();
        services.AddSingleton<IConeFactory, ConeFactory>();
        services.AddSingleton<ILineFactory, LineFactory>();
        services.AddSingleton<IPolygonFactory, PolygonFactory>();   
    }
}
