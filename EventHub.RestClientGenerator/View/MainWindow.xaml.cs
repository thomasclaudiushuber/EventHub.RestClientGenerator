using System.Diagnostics;
using System.Windows;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.ViewModel;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.View
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainViewModel();
    }

    private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
  }
}
