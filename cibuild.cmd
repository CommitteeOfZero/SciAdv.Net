call "SetDevCommandPrompt.cmd"
nuget restore
msbuild /p:CIBuild=true /p:Configuration=Release /maxcpucount /m