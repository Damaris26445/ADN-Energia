using Microsoft.Data.SqlClient;
using TransportesNorte.Models;

namespace TransportesNorte.Services;

/// <summary>
/// Implementacion real del servicio: habla con SQL Server.
/// Usa async/await en cada operacion para no bloquear la interfaz.
/// </summary>
public class DespachoService : IDespachoService
{
    private readonly string _connectionString;

    public DespachoService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<DespachoSalida>> GetDespachosPendientesAsync()
    {
        var despachos = new List<DespachoSalida>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Solo traemos los que NO tienen peso de bascula (pendientes de pesaje)
        const string sql = @"
            SELECT ID, Folio_Despacho, Centro_Operativo,
                   Placa_Tracto, Nombre_Conductor,
                   Peso_Tara, Peso_Teorico_ERP
            FROM   dbo.Despachos_Salida
            WHERE  Peso_Bascula_Salida IS NULL
            ORDER BY Folio_Despacho";

        using var cmd    = new SqlCommand(sql, connection);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            despachos.Add(new DespachoSalida
            {
                ID               = reader.GetInt32(0),
                Folio_Despacho   = reader.GetString(1),
                Centro_Operativo = reader.GetString(2),
                Placa_Tracto     = reader.GetString(3),
                Nombre_Conductor = reader.GetString(4),
                Peso_Tara        = reader.GetDecimal(5),
                Peso_Teorico_ERP = reader.GetDecimal(6)
            });
        }

        return despachos;
    }

    public async Task AutorizarSalidaAsync(
        string  folioDespacho,
        decimal pesoBascula,
        decimal pesoNeto,
        string? justificacion)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var cmd = new SqlCommand("dbo.sp_AutorizarSalida", connection)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Folio_Despacho",           folioDespacho);
        cmd.Parameters.AddWithValue("@Peso_Bascula_Salida",      pesoBascula);
        cmd.Parameters.AddWithValue("@Peso_Neto_Real",           pesoNeto);
        cmd.Parameters.AddWithValue("@Justificacion_Diferencia", (object?)justificacion ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync();
    }
}
