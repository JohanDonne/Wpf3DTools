using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Demo04_Movement.Presentation;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private Point _lastPoint;

    public MainWindow(MainViewModel vm)
    {
        _viewModel = vm;
        DataContext = _viewModel;
        InitializeComponent();
        PreviewKeyDown += WindowKeyDown;
        viewportControl.MouseDown += ViewPortMouseDown;
        viewportControl.PreviewMouseWheel += ViewPortPreviewMouseWheel;
        CompositionTarget.Rendering += UpdateContent;
    }

    private void UpdateContent(object? sender, EventArgs e)
    {
        _viewModel.Update();
    }

    #region camera control

    private void WindowKeyDown(object sender, KeyEventArgs e)
    {
        _viewModel.ProcessKey(e.Key);
    }

    private void ViewPortPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        _viewModel.Zoom(e.Delta);
    }

    private void ViewPortMouseDown(object sender, MouseButtonEventArgs e)
    {
        _lastPoint = e.GetPosition(mainViewport);
        _ = viewportControl.CaptureMouse();
        viewportControl.MouseUp += ViewPortMouseUp;
        viewportControl.PreviewMouseMove += ViewPortMouseMove;
    }

    private void ViewPortMouseMove(object sender, MouseEventArgs e)
    {
        var newPoint = e.GetPosition(mainViewport);
        var vector = newPoint - _lastPoint;
        _viewModel.ControlByMouse(vector);
        _lastPoint = newPoint;
    }

    private void ViewPortMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewportControl.ReleaseMouseCapture();
        viewportControl.PreviewMouseMove -= ViewPortMouseMove;
        viewportControl.MouseUp -= ViewPortMouseUp;
    }

    #endregion camera control

}
