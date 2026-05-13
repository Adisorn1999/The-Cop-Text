using System.Windows;
using WpfApp1.Data;

namespace WpfApp1;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(
        System.Windows.StartupEventArgs e)
    {
        base.OnStartup(e);

        var db =
            new SQLiteService();

        db.Initialize();
    }
}