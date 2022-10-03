using Microsoft.Extensions.DependencyInjection;

namespace Wpf3dDemo.PresentationLayer;

public static class ServiceExtensions
{
    public static void AddPresentationServices(this ServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddTransient<MainViewModel>();
    }
}
