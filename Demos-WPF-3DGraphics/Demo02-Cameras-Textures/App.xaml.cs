using Demo02_Cameras_Textures.Presentation;
using System.Windows;
using Wpf3dTools.Factories;
using Wpf3dTools.Implementation;

namespace Demo02_Cameras_Textures;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        new MainWindow(new MainViewModel(new Models.World(), new SphericalCameraController(), new ShapesFactory())).Show();
    }
}
