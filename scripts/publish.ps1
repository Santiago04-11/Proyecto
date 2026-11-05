Write-Host "Iniciando empaquetado de GTN-API para Windows x64..." -ForegroundColor Cyan

dotnet publish src/GtnApi.Desktop -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish

Write-Host "¡Empaquetado completado! Revisa la carpeta 'publish'." -ForegroundColor Green
