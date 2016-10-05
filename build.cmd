call "SetDevCommandPrompt.cmd"
nuget restore
msbuild /p:Configuration=Release /maxcpucount /m