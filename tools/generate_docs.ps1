dotnet restore "../src/Snippetica.CodeGeneration.DocumentationGenerator/DocumentationGenerator.csproj" --force
dotnet build "../src/Snippetica.CodeGeneration.DocumentationGenerator/DocumentationGenerator.csproj" --no-restore /p:"Configuration=Release,Deterministic=true,TreatWarningsAsErrors=true,WarningsNotAsErrors=1591" /m

if(!$?) { Read-Host; Exit }

& "../src/Snippetica.CodeGeneration.DocumentationGenerator/bin/Release/net6.0/Snippetica.CodeGeneration.DocumentationGenerator" "../src" "build" "../src/Snippetica.CodeGeneration.Metadata/data"
