@echo off

"C:\Program Files\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild" "..\Snippetica.sln" ^
 /t:Build ^
 /p:Configuration=Debug ^
 /v:normal ^
 /fl ^
 /m

if errorlevel 1 (
 pause
 exit
)

echo OK
pause
