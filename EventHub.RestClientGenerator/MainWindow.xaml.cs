using System.Windows;
using ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.ViewModel;

namespace EventHub.RestClientGenerator
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainViewModel();
    }
  }
}
