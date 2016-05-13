using System.Windows;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      MessageBox.Show("Exception: " + e.Exception.Message);
      e.Handled = true;
    }
  }
}
