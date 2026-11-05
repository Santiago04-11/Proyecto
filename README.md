# GTN-API

Gestor de Tareas y Notas **100% offline** para Windows, con una **API HTTP local** para integrarlo con otros programas (scripts, herramientas de línea de comandos, etc.).

> Estado: en desarrollo (M0: setup inicial)

## Características previstas

- CRUD de **Notas**, **Tareas** y **Proyectos** desde una interfaz WPF.
- API REST local (JSON) disponible únicamente en `127.0.0.1`.
- Persistencia local con SQLite.
- Sin telemetría, sin auto-update, sin llamadas a internet en runtime.

## Stack

| Capa         | Tecnología               |
|--------------|--------------------------|
| Lenguaje     | C# / .NET 8              |
| GUI          | WPF                      |
| API local    | ASP.NET Core Minimal API |
| Persistencia | SQLite + EF Core         |
| Tests        | xUnit                    |

## Estructura

@'
# GTN-API

Gestor de Tareas y Notas **100% offline** para Windows, con una **API HTTP local** para integrarlo con otros programas (scripts, herramientas de línea de comandos, etc.).

> Estado: en desarrollo (M0: setup inicial)

## Características previstas

- CRUD de **Notas**, **Tareas** y **Proyectos** desde una interfaz WPF.
- API REST local (JSON) disponible únicamente en `127.0.0.1`.
- Persistencia local con SQLite.
- Sin telemetría, sin auto-update, sin llamadas a internet en runtime.

## Stack

| Capa         | Tecnología               |
|--------------|--------------------------|
| Lenguaje     | C# / .NET 8              |
| GUI          | WPF                      |
| API local    | ASP.NET Core Minimal API |
| Persistencia | SQLite + EF Core         |
| Tests        | xUnit                    |

## Estructura

Proyecto/
├─ docs/ # SRS y documentación de la API
├─ src/
│ ├─ GtnApi.Core/ # modelos, DbContext, repositorios
│ ├─ GtnApi.Api/ # Minimal API
│ └─ GtnApi.Desktop/ # WPF
├─ tests/
│ └─ GtnApi.Tests/ # xUnit
└─ scripts/ # publish.ps1


## Requisitos de desarrollo

- Windows 10/11
- .NET SDK 8.0
- Git

## Compilar y probar

```powershell
dotnet restore
dotnet build
dotnet test
```

## Documentación

- [SRS (IEEE 830)](docs/SRS.md)
- [API](docs/API.md) (pendiente)

## Roadmap

- [x] M0: Setup
- [ ] M1: Núcleo de datos
- [ ] M2: API local
- [ ] M3: GUI WPF
- [ ] M4: Empaquetado
- [ ] M5: Documentación

## Licencia

[MIT](LICENSE)
