$msbuild = ${env:ProgramFiles(x86)} + '\MSBuild\14.0\Bin\MSBuild.exe'
& $msbuild SciAdvNet.sln /p:SolutionDir=$PSScriptRoot /p:Configuration=Release /m

Invoke-Item 'releases'