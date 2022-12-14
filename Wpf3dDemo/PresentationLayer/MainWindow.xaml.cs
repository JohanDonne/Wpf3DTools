using System.Windows;
using System.Windows.Input;

namespace Wpf3dDemo.PresentationLayer;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private readonly MainViewModel _viewModel;
    private Point _lastPoint;
    public MainWindow(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    #region camera control

    private void ViewPortMouseDown(object sender, MouseButtonEventArgs e)
    {
        _lastPoint = e.GetPosition(mainViewPort);
        _ = viewPortControl.CaptureMouse();
        viewPortControl.MouseUp += ViewPortMouseUp;
        viewPortControl.PreviewMouseMove += ViewPortMouseMove;
    }

    private void ViewPortMouseMove(object sender, MouseEventArgs e)
    {
        var newPoint = e.GetPosition(mainViewPort);
        var vector = newPoint - _lastPoint;
        _viewModel.ControlByMouseCommand.Execute(vector);
        _lastPoint = newPoint;
    }

    private void ViewPortMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewPortControl.ReleaseMouseCapture();
        viewPortControl.PreviewMouseMove -= ViewPortMouseMove;
        viewPortControl.MouseUp -= ViewPortMouseUp;
    }

    #endregion camera control

}
