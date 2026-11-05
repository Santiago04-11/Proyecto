# GTN-API 📝

Gestor de Tareas y Notas **100% offline** para Windows, con una **API HTTP local** para integrarlo con otros programas (scripts, herramientas de línea de comandos, etc.).

> Estado: **Completado (M5)** — Aplicación funcional y empaquetada.

## Características

- CRUD de **Notas**, **Tareas** y **Proyectos** desde una interfaz WPF.
- API REST local (JSON) disponible únicamente en `127.0.0.1` (Puerto `5199`).
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

## Estructura del Proyecto

- `docs/` -> SRS y documentación de la API
- `src/`
  - `GtnApi.Core/` -> Modelos, DbContext, repositorios
  - `GtnApi.Api/` -> Minimal API
  - `GtnApi.Desktop/` -> Interfaz WPF
- `tests/`
  - `GtnApi.Tests/` -> Pruebas unitarias
- `scripts/` -> publish.ps1

## Requisitos de desarrollo

- Windows 10/11
- .NET SDK 8.0
- Git

## Compilar y probar

Ejecuta los siguientes comandos en la raíz del proyecto:

dotnet restore
dotnet build
dotnet test

## 🚀 Generar el ejecutable final (.exe)

Para crear un archivo ejecutable autocontenido (que no requiere tener .NET instalado en la máquina destino):

.\scripts\publish.ps1

El ejecutable quedará en la carpeta `publish/`.

## 🌐 Documentación de la API

La API cuenta con Swagger para pruebas interactivas. Para encenderla:

dotnet run --project src/GtnApi.Api

Luego abre en tu navegador: `http://127.0.0.1:5199/swagger`

- [SRS (IEEE 830)](docs/SRS.md)
- [API](docs/API.md)

## Roadmap

- [x] M0: Setup
- [x] M1: Núcleo de datos
- [x] M2: API local
- [x] M3: GUI WPF
- [x] M4: Empaquetado
- [x] M5: Documentación

## Licencia

[MIT](LICENSE)