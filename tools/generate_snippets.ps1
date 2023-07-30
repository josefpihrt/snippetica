dotnet restore "../src/Tools/SnippetGenerator/SnippetGenerator.csproj" --force
dotnet build "../src/Tools/SnippetGenerator/SnippetGenerator.csproj" --no-restore /p:"Configuration=Release,Deterministic=true,TreatWarningsAsErrors=true,WarningsNotAsErrors=1591" /m

if(!$?) { Read-Host; Exit }

& "../src/Tools/SnippetGenerator/bin/Release/net6.0/Snippetica.CodeGeneration.SnippetGenerator" "../src" "../src/Snippetica.Metadata/data"
