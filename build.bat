echo off

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

REM Restore packages
call "%nuget%" restore "UXC Solution.sln" -NonInteractive

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" "UXC Solution.sln" /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build

for %%p in (
    UXC.Core.Interfaces
    UXC.Core.Data
    UXC.Core.Data.Serialization
    UXC.Core.Data.Serialization.Json
) do (
 	call "%nuget%" pack ".nuget\%%p.nuspec" -symbols -o Build -p Configuration=%config% 
)
