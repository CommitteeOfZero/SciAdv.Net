@echo off
setlocal

for /f "tokens=*" %%i in ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -prerelease -property installationPath') do set VSPATH=%%i
set msbuild=%VSPATH%\MSBuild\15.0\Bin\MSBuild.exe

"%msbuild%" SciAdvNet.sln /t:restore /p:Configuration=Release
"%msbuild%" SciAdvNet.sln /p:Configuration=Release /nologo /maxcpucount /m /v:m
pause