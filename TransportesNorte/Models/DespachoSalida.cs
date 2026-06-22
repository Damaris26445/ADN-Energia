namespace TransportesNorte.Models;

/// <summary>
/// Representa un despacho de camion en el CEDIS.
/// Los campos nullable son los que la App llena al autorizar la salida.
/// </summary>
public class DespachoSalida
{
    public int     ID                       { get; set; }
    public string  Folio_Despacho           { get; set; } = string.Empty;
    public string  Centro_Operativo         { get; set; } = string.Empty;
    public string  Placa_Tracto             { get; set; } = string.Empty;
    public string  Nombre_Conductor         { get; set; } = string.Empty;
    public decimal Peso_Tara                { get; set; }
    public decimal Peso_Teorico_ERP         { get; set; }

    // Estos campos son NULL hasta que el operador autoriza la salida
    public decimal? Peso_Bascula_Salida      { get; set; }
    public decimal? Peso_Neto_Real           { get; set; }
    public string?  Justificacion_Diferencia { get; set; }
    public DateTime? Fecha_Hora_Salida       { get; set; }
}
