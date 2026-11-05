using System.Windows;
using GtnApi.Core;
using GtnApi.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GtnApi.Desktop;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var serviceCollection = new ServiceCollection();
        
        serviceCollection.AddGtnCore();
        serviceCollection.AddSingleton<MainWindow>();

        Services = serviceCollection.BuildServiceProvider();

        // Asegurarnos de que la base de datos exista al abrir la app
        using (var scope = Services.CreateScope())
        {
            var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GtnDbContext>>();
            DatabaseInitializer.MigrateAsync(factory).GetAwaiter().GetResult();
        }

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
