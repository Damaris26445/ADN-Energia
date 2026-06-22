using System.Windows;
using Microsoft.Extensions.Configuration;
using TransportesNorte.Services;
using TransportesNorte.ViewModels;
using TransportesNorte.Views;

namespace TransportesNorte;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Lee la cadena de conexion desde appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "La cadena de conexion 'Default' no esta configurada en appsettings.json.");

        // Composicion de dependencias: creamos el servicio una sola vez
        // y lo inyectamos en el ViewModel
        var service   = new DespachoService(connectionString);
        var viewModel = new DashboardViewModel(service);

        new MainWindow(viewModel).Show();
    }
}
