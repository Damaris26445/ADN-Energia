using System.IO;

namespace TransportesNorte.Helpers;

/// <summary>
/// Escribe errores tecnicos en log.txt junto al ejecutable.
/// Punto extra de la prueba tecnica.
/// </summary>
public static class Logger
{
    private static readonly string LogPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "log.txt");

    public static void LogError(string mensaje, Exception? ex = null)
    {
        var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {mensaje}";
        if (ex is not null)
            linea += $" | {ex.GetType().Name}: {ex.Message}";

        try
        {
            File.AppendAllText(LogPath, linea + Environment.NewLine);
        }
        catch
        {
            // Si no se puede escribir el log, no queremos que se caiga la App
        }
    }
}
