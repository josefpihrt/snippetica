dotnet restore "../src/Snippetica.CodeGeneration.SnippetGenerator/SnippetGenerator.csproj" --force
dotnet build "../src/Snippetica.CodeGeneration.SnippetGenerator/SnippetGenerator.csproj" --no-restore /p:"Configuration=Release,Deterministic=true,TreatWarningsAsErrors=true,WarningsNotAsErrors=1591" /m

if(!$?) { Read-Host; Exit }

& "../src/Snippetica.CodeGeneration.SnippetGenerator/bin/Release/net6.0/Snippetica.CodeGeneration.SnippetGenerator" "../src" "../src/Snippetica.CodeGeneration.Metadata/data"
