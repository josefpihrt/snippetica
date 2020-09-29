@echo off

dotnet restore --force "..\Snippetica.sln"

"C:\Program Files\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild" "..\Snippetica.sln" ^
 /t:Clean,Build ^
 /p:Configuration=Release,TreatWarningsAsErrors=true,WarningsNotAsErrors=1591 ^
 /v:normal ^
 /fl ^
 /m

if errorlevel 1 (
 pause
 exit
)

echo OK
pause
