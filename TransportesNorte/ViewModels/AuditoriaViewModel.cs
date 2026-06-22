using System.Globalization;
using TransportesNorte.Helpers;
using TransportesNorte.Models;
using TransportesNorte.Services;

namespace TransportesNorte.ViewModels;

public class AuditoriaViewModel : BaseViewModel
{
    private readonly IDespachoService   _service;
    private readonly DashboardViewModel _dashboardVm;
    private readonly DespachoSalida     _despacho;

    // ── Datos de solo lectura (vienen del registro seleccionado) ──────────
    public string  FolioDespacho   => _despacho.Folio_Despacho;
    public string  CentroOperativo => _despacho.Centro_Operativo;
    public string  PlacaTracto     => _despacho.Placa_Tracto;
    public string  NombreConductor => _despacho.Nombre_Conductor;
    public decimal PesoTara        => _despacho.Peso_Tara;
    public decimal PesoTeoricoERP  => _despacho.Peso_Teorico_ERP;

    // ── Input del operador ────────────────────────────────────────────────
    private string _pesoBasculaTexto = string.Empty;
    public string PesoBasculaTexto
    {
        get => _pesoBasculaTexto;
        set
        {
            if (SetProperty(ref _pesoBasculaTexto, value))
                Recalcular(); // Recalcula en cada tecla que presiona el usuario
        }
    }

    private decimal? _pesoBascula;

    // ── Campos calculados ─────────────────────────────────────────────────
    private decimal? _pesoNetoReal;
    public decimal? PesoNetoReal
    {
        get => _pesoNetoReal;
        private set => SetProperty(ref _pesoNetoReal, value);
    }

    private double _diferenciaPct;
    public double DiferenciaPct
    {
        get => _diferenciaPct;
        private set => SetProperty(ref _diferenciaPct, value);
    }

    // ── Validacion critica del 3% ─────────────────────────────────────────
    private bool _mostrarAlerta;
    public bool MostrarAlerta
    {
        get => _mostrarAlerta;
        private set => SetProperty(ref _mostrarAlerta, value);
    }

    private string _justificacion = string.Empty;
    public string Justificacion
    {
        get => _justificacion;
        set
        {
            if (SetProperty(ref _justificacion, value))
                AutorizarSalidaCommand.RaiseCanExecuteChanged();
        }
    }

    // ── Estado de la operacion ────────────────────────────────────────────
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
                AutorizarSalidaCommand.RaiseCanExecuteChanged();
        }
    }

    private string _mensajeExito = string.Empty;
    public string MensajeExito
    {
        get => _mensajeExito;
        private set => SetProperty(ref _mensajeExito, value);
    }

    private string _mensajeError = string.Empty;
    public string MensajeError
    {
        get => _mensajeError;
        private set => SetProperty(ref _mensajeError, value);
    }

    private bool _salidaAutorizada;
    public bool SalidaAutorizada
    {
        get => _salidaAutorizada;
        private set
        {
            if (SetProperty(ref _salidaAutorizada, value))
                AutorizarSalidaCommand.RaiseCanExecuteChanged();
        }
    }

    public RelayCommand AutorizarSalidaCommand { get; }

    public AuditoriaViewModel(
        DespachoSalida     despacho,
        IDespachoService   service,
        DashboardViewModel dashboardVm)
    {
        _despacho    = despacho;
        _service     = service;
        _dashboardVm = dashboardVm;

        AutorizarSalidaCommand = new RelayCommand(
            async _ => await AutorizarAsync(),
                  _  => PuedeAutorizar()
        );
    }

    // ── Logica de calculo en tiempo real ──────────────────────────────────
    private void Recalcular()
    {
        // Acepta punto o coma como separador decimal; permite valores negativos
        var texto = _pesoBasculaTexto.Replace(',', '.');

        if (decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bascula))
        {
            _pesoBascula = bascula;
            PesoNetoReal = bascula - _despacho.Peso_Tara;

            if (_despacho.Peso_Teorico_ERP != 0)
            {
                DiferenciaPct = Math.Abs(
                    (double)(PesoNetoReal.Value - _despacho.Peso_Teorico_ERP)
                    / (double)_despacho.Peso_Teorico_ERP * 100.0);

                MostrarAlerta = DiferenciaPct > 3.0;
            }
        }
        else
        {
            _pesoBascula  = null;
            PesoNetoReal  = null;
            DiferenciaPct = 0;
            MostrarAlerta = false;
        }

        AutorizarSalidaCommand.RaiseCanExecuteChanged();
    }

    private bool PuedeAutorizar()
    {
        if (IsBusy || SalidaAutorizada)                          return false;
        if (_pesoBascula is null || PesoNetoReal is null)        return false;
        if (MostrarAlerta && string.IsNullOrWhiteSpace(Justificacion)) return false;
        return true;
    }

    // ── Guardado asíncrono ────────────────────────────────────────────────
    private async Task AutorizarAsync()
    {
        try
        {
            IsBusy       = true;
            MensajeError = string.Empty;

            await _service.AutorizarSalidaAsync(
                _despacho.Folio_Despacho,
                _pesoBascula!.Value,
                PesoNetoReal!.Value,
                MostrarAlerta ? Justificacion : null
            );

            SalidaAutorizada = true;
            MensajeExito     = $"Salida autorizada correctamente — {_despacho.Folio_Despacho}";

            // Actualiza el Dashboard sin recargar toda la lista desde BD
            _dashboardVm.RemoverDespacho(_despacho.Folio_Despacho);
        }
        catch (Exception ex)
        {
            MensajeError = "Error de conexion con la base de datos. Intente nuevamente.";
            Logger.LogError($"Error al autorizar salida {_despacho.Folio_Despacho}", ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
