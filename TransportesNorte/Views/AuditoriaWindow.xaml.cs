using System.Windows;
using TransportesNorte.ViewModels;

namespace TransportesNorte.Views;

public partial class AuditoriaWindow : Window
{
    public AuditoriaWindow(AuditoriaViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}
