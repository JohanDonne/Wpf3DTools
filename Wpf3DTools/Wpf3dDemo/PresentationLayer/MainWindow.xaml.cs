using System.Windows;
using Wpf3dDemo.LogicLayer;

namespace Wpf3dDemo.PresentationLayer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
        public MainWindow(MainViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
