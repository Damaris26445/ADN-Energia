using System.Windows;
using TransportesNorte.Models;
using TransportesNorte.ViewModels;

namespace TransportesNorte.Views;

public partial class MainWindow : Window
{
    private readonly DashboardViewModel _vm;

    public MainWindow(DashboardViewModel vm)
    {
        InitializeComponent();
        _vm         = vm;
        DataContext = vm;

        // Cuando el ViewModel senala que hay que auditar un despacho,
        // esta ventana abre la ventana de auditoria como dialogo modal
        _vm.SolicitarAuditoria += AbrirAuditoria;

        Loaded += async (_, _) => await _vm.CargarAsync();
    }

    private void AbrirAuditoria(DespachoSalida despacho)
    {
        var auditoriaVm = new AuditoriaViewModel(despacho, _vm.Service, _vm);
        var ventana     = new AuditoriaWindow(auditoriaVm) { Owner = this };
        ventana.ShowDialog();
    }
}
