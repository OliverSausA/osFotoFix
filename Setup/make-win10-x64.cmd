
set NSIS="C:\Program Files (x86)\NSIS\makensis.exe"
set BUILD=win10-x64

if exist %BUILD%-deployment del %BUILD%-deployment\*.* -Y
if not exist %BUILD%-setup mkdir %BUILD%-setup

dotnet publish ..\osFotoFix -o %BUILD%-deployment -r %BUILD% -p:PublishReadyRun=true -p:PublishSingleFile=false -p:PublishedTrim=true --self-contained true

%NSIS% %BUILD%.nsi

