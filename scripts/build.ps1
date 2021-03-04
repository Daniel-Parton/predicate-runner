$srcPath = Join-Path -Path $PSScriptRoot -ChildPath "..\src"

# Build solution
dotnet build (Join-Path -Path $srcPath -ChildPath "Predicator.sln") -c "Release"
if ($LASTEXITCODE -ne 0) { throw "broken build" }