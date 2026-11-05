namespace GtnApi.Core.Data;

/// <summary>Ubicación por defecto de la base de datos local del usuario.</summary>
public static class DatabasePaths
{
    public static string DefaultDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GtnApi");

    public static string DefaultDatabasePath => Path.Combine(DefaultDirectory, "gtn.db");
}
