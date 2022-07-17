@echo off

set _programFiles=%ProgramFiles%

set _roslynatorPath=..\..\Roslynator\src
set _visualStudioPath=%_programFiles%\Microsoft Visual Studio\2022\Enterprise
set _msbuildPath=%_visualStudioPath%\MSBuild\Current\Bin

"%_msbuildPath%\msbuild" "%_roslynatorPath%\CommandLine.sln" /t:Build /p:Configuration=Debug /v:m /m

"%_roslynatorPath%\CommandLine\bin\Debug\net48\roslynator" fix "..\src\Snippetica.sln" ^
 --msbuild-path "%_msbuildPath%" ^
 --analyzer-assemblies ^
  "%_roslynatorPath%\Analyzers.CodeFixes\bin\Debug\netstandard2.0\Roslynator.CSharp.Analyzers.dll" ^
  "%_roslynatorPath%\Analyzers.CodeFixes\bin\Debug\netstandard2.0\Roslynator.CSharp.Analyzers.CodeFixes.dll" ^
  "%_roslynatorPath%\Formatting.Analyzers.CodeFixes\bin\Debug\netstandard2.0\Roslynator.Formatting.Analyzers.dll" ^
  "%_roslynatorPath%\Formatting.Analyzers.CodeFixes\bin\Debug\netstandard2.0\Roslynator.Formatting.Analyzers.CodeFixes.dll" ^
 --ignored-projects Snippetica.Test ^
 --format ^
 --verbosity d ^
 --file-log "roslynator.log" ^
 --file-log-verbosity diag ^
 --diagnostic-fix-map "RCS1155=Roslynator.RCS1155.OrdinalIgnoreCase" ^
 --file-banner " Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information."

pause
