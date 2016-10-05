call "SetDevCommandPrompt.cmd"
nuget restore
msbuild /p:CIBuild=false /p:Configuration=Release /maxcpucount /m