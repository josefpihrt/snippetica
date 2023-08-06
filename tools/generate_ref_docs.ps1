$roslynatorExe="../../roslynator/src/CommandLine/bin/Debug/net7.0/Roslynator"

dotnet restore "../../Roslynator/src/CommandLine.sln" /p:Configuration=Debug -v m
dotnet build "../../Roslynator/src/CommandLine.sln" --no-restore /p:Configuration=Debug /v:m /m

 & $roslynatorExe generate-doc "../src/Snippetica.VisualStudio/Snippetica.VisualStudio.csproj" `
 --properties "Configuration=Release" `
 --heading ".NET API Reference" `
 -o "build/ref" `
 --host docusaurus `
 --group-by-common-namespace `
 --ignored-common-parts content `
 --ignored-root-parts all `
 --max-derived-types 10 `
 --ignored-names "Snippetica.VisualStudio.Serialization"

& $roslynatorExe generate-doc-root "../src/Snippetica.VisualStudio/Snippetica.VisualStudio.csproj" `
  --properties "Configuration=Release" `
  -o "build/ref.md" `
  --host docusaurus `
  --heading ".NET API Reference" `
  --ignored-parts content `
  --group-by-common-namespace `
  --root-directory-url "ref" `
 --ignored-names "Snippetica.VisualStudio.Serialization"

Write-Host "DONE"
