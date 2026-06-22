using System.Windows.Input;

namespace TransportesNorte.Helpers;

/// <summary>
/// Implementacion de ICommand que conecta los botones de la Vista
/// con metodos del ViewModel. Sustituye el code-behind en WPF MVVM.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute    = execute;
        _canExecute = canExecute;
    }

    // WPF llama a CanExecuteChanged para saber si re-evaluar el boton
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);

    // El ViewModel llama a este metodo cuando alguna propiedad cambia
    // y puede afectar si el boton debe estar habilitado o no
    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
