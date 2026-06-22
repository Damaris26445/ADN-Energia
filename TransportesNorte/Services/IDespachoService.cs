using TransportesNorte.Models;

namespace TransportesNorte.Services;

/// <summary>
/// Contrato (interfaz) del servicio de datos.
/// Separar interfaz de implementacion permite cambiar
/// la fuente de datos (SQL, mock, API) sin tocar los ViewModels.
/// </summary>
public interface IDespachoService
{
    Task<List<DespachoSalida>> GetDespachosPendientesAsync();

    Task AutorizarSalidaAsync(
        string  folioDespacho,
        decimal pesoBascula,
        decimal pesoNeto,
        string? justificacion);
}
