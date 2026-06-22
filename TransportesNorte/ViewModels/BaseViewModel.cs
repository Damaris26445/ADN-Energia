using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransportesNorte.ViewModels;

/// <summary>
/// Clase base para todos los ViewModels.
/// Implementa INotifyPropertyChanged, que es el mecanismo que
/// le avisa a la Vista cuando una propiedad cambia para que
/// actualice la pantalla automaticamente (esto es el "binding").
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Asigna un nuevo valor a un campo y notifica a la Vista si cambio.
    /// Retorna true si el valor fue diferente (hubo cambio real).
    /// </summary>
    protected bool SetProperty<T>(ref T campo, T valor, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(campo, valor)) return false;
        campo = valor;
        OnPropertyChanged(propertyName);
        return true;
    }
}
