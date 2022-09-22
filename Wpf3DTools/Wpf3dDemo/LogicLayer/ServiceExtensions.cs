using Microsoft.Extensions.DependencyInjection;
using Wpf3dDemo.Domain;

namespace Wpf3dDemo.LogicLayer;

public static class ServiceExtensions
{
    public static void AddLogicServices(this ServiceCollection services)
    {
        services.AddTransient<ILogic, Logic>();
    }
}
