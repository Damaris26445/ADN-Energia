using System.Collections.ObjectModel;
using TransportesNorte.Helpers;
using TransportesNorte.Models;
using TransportesNorte.Services;

namespace TransportesNorte.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly IDespachoService _service;
    public IDespachoService Service => _service;

    // ObservableCollection notifica a la Vista automaticamente
    // cuando se agregan o quitan elementos de la lista
    public ObservableCollection<DespachoSalida> Despachos { get; } = [];

    private DespachoSalida? _selectedDespacho;
    public DespachoSalida? SelectedDespacho
    {
        get => _selectedDespacho;
        set
        {
            if (SetProperty(ref _selectedDespacho, value))
                AbrirAuditoriaCommand.RaiseCanExecuteChanged();
        }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public RelayCommand CargarDespachosCommand { get; }
    public RelayCommand AbrirAuditoriaCommand  { get; }

    // Evento que dispara la apertura de la ventana de Auditoria.
    // El code-behind de MainWindow se suscribe a este evento.
    public event Action<DespachoSalida>? SolicitarAuditoria;

    public DashboardViewModel(IDespachoService service)
    {
        _service = service;

        CargarDespachosCommand = new RelayCommand(
            async _ => await CargarAsync());

        AbrirAuditoriaCommand = new RelayCommand(
            _ =>
            {
                if (SelectedDespacho is not null)
                    SolicitarAuditoria?.Invoke(SelectedDespacho);
            },
            _ => SelectedDespacho is not null
        );
    }

    public async Task CargarAsync()
    {
        try
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            var lista = await _service.GetDespachosPendientesAsync();

            Despachos.Clear();
            foreach (var d in lista)
                Despachos.Add(d);
        }
        catch (Exception ex)
        {
            ErrorMessage = "No se pudo conectar a la base de datos. Verifique la conexion e intente de nuevo.";
            Logger.LogError("Error al cargar despachos pendientes", ex);
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Quita un despacho de la lista local despues de autorizarlo,
    // sin necesidad de recargar toda la lista desde BD
    public void RemoverDespacho(string folioDespacho)
    {
        var item = Despachos.FirstOrDefault(d => d.Folio_Despacho == folioDespacho);
        if (item is not null)
            Despachos.Remove(item);
    }
}
