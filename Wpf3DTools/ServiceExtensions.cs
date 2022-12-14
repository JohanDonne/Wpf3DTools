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
        services.AddSingleton<IShapesFactory, ShapesFactory>();
    }
}
