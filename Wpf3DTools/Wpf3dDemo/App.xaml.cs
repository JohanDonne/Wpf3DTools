using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Wpf3dDemo.LogicLayer;
using Wpf3dDemo.PresentationLayer;
using Wpf3dTools;

namespace Wpf3dDemo;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddWpf3dServices();
        services.AddLogicServices();
        services.AddPresentationServices();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainwWindow = _serviceProvider.GetService<MainWindow>();
        mainwWindow?.Show();
    }
}
