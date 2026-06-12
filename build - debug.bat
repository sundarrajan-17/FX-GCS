
set PATH=%PATH%;C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin;C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin;C:\Program Files (x86)\Microsoft Visual Studio\Preview\Community\MSBuild\15.0\Bin;C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin

del bin\release\XagSurveillanceGCSBeta.zip

.nuget\nuget.exe restore XagSurveillanceGCS.sln

MSBuild.exe XagSurveillanceGCS.sln /restore /m /p:Configuration=Debug /verbosity:n

pause