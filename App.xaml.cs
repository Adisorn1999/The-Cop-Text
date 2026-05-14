using System.Windows;
using WpfApp1.Data;

namespace WpfApp1;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(
        StartupEventArgs e)
    {
        base.OnStartup(e);

        var db =
            new SQLiteService();

        db.Initialize();
    }

    private void OpenApp_Click(
        object sender,
        RoutedEventArgs e)
    {
        MainWindow window =
            (MainWindow)Current.MainWindow;

        window.Show();

        window.WindowState =
            WindowState.Normal;

        window.Activate();
    }

    private void ExitApp_Click(
        object sender,
        RoutedEventArgs e)
    {
        MainWindow window =
            (MainWindow)Current.MainWindow;

        window.AllowClose();

        System.Windows.Application.Current.Shutdown();
    }
}