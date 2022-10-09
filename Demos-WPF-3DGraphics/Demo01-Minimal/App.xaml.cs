using Demo01_Minimal.Presentation;
using System.Windows;

namespace Demo01_Minimal;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override void OnStartup(StartupEventArgs e)
    {
        new MainWindow(new MainViewModel(new Models.World())).Show();
    }
}
