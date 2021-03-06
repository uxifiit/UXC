﻿# <img src="docs/logo.png" height="28" /> UXC 

[![Build Status](https://dev.azure.com/uxifiit/UXC/_apis/build/status/uxifiit.UXC?branchName=master)](https://dev.azure.com/uxifiit/UXC/_build/latest?definitionId=7&branchName=master)

###### Client application of the UXI Group Studies infrastructure.

UXC is an application for conducting user studies with eye tracking for Microsoft Windows.
UXC client and UXR web management applications form the UXI Group Studies infrastructure for conducting group eye tracking studies. 
This project is developed at [User eXperience and Interaction Research Center](https://www.uxi.sk/) of [Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava](http://fiit.stuba.sk/).

For more information about the infrastructure, group studies and our lab, see the [Publications](#publications) section below.

See [project Wiki](https://github.com/uxifiit/UXC/wiki) for documentation, examples and guides.


![UXC](/docs/uxc.png)


## Main features 

* Recording user studies with eye tracking
* Stimuli timeline and playback
* Connection with the **[UXR management application](https://github.com/uxifiit/UXR/)** for:
  + Remote setup and control of the session recording,
  + Upload recording data on session finish.
* Integration options for 3rd party applications used in the experiment, e.g., a web or desktop application through locally hosted REST API and web sockets, providing them with:
  + Real-time access to eye tracking and other session recording data 
  + Session recording control, i.e., to start/stop recording, query status, or control stimuli timeline playback
  + Writing custom events


### Session recording options

Session recording can be started and controlled in 3 ways:
* Remotely, by the study conductor through the UXR management application,
* Locally, using the application user interface, or
* Through the REST API or web socket with SignalR by a 3rd party application.


### Supported devices

UXC can record data from these devices during the study:

* Tobii Pro eye trackers - using the [Tobii Pro SDK](http://developer.tobiipro.com/) - Tobii X2-60 and TX300 were tested,
* Webcam video and audio recording - using the [FFmpeg](https://www.ffmpeg.org/) binary,
* Screencast video recording - using the [UScreenCapture](http://www.umediaserver.net/umediaserver/download.html) utility and the FFmpeg binary,
* Keyboard and mouse events logging - using the Windows API hooks,
* External events logging - using the locally hosted REST API and websockets with [ASP.NET SignalR](https://www.asp.net/signalr).


### Stimuli timeline

These types of stimuli are provided for playback during the session recording:

* Eye tracker calibration with customizable calibration plans
* Eye tracker validation
* Instructions
* Questionary with write or choose answer questions
* Show image
* Show desktop
* Launch program
* Introduction into experiment


## Build and setup

UXC is a GUI application for Microsoft Windows written in C# v6.0 and .NET v4.5.2.
To build the UXC and other projects in this repository, follow these steps:

* Install Microsoft Visual Studio 2015 or 2017, or Visual Studio Build Tools.
* To build the solution using the Visual Studio user interface:
    + Open the `UXC Solution.sln` file.
	+ Set up build target to `Release`.
	+ Build the solution (default hotkey <kbd>F6</kbd>).
* To build the solution using the command line: 
    + Download [NuGet Windows Commandline](https://www.nuget.org/downloads), v4.9.x were tested.
	+ Create new environment variable with name `nuget` and its path set to the `nuget.exe` executable, e.g., `C:\Program Files (x86)\NuGet\nuget.exe`.
    + Test the path in a new command line window with `echo %nuget%`.
	+ Run the `build.bat` script.
* Locate build output of the UXC in the `/src/apps/UXC/build/Release/` directory.

Before running the UXC application, follow these steps to set it up:

* Enable opening port `55555` on the firewall, see the [Firewall Setup](https://github.com/uxifiit/UXC/wiki/Firewall-Setup) page in the Wiki.
* For screencast recording, download and install the *x86 version* of the [UScreenCapture](http://www.umediaserver.net/umediaserver/download.html).
* Configure webcam video and audio devices in the `UXC.Devices.Streamers.ini` configuration file.
* Configure UXR endpoint in the `UXC.Plugins.UXR.ini` configuration file.

Then launch the `UXC.exe` executable.


## Repository contents

Source code folder `src` structure:

* `docs` - documentation files, images, sample session definitions
* `src` - source code of the UXC application and its components:
  + `apps` - the main UXC app project
  + `core` - core libraries of the application
  + `devices` - libraries implementing communication with recording devices
  + `plugins` - optional plugins for the application
* `test` - source code of testing projects



## NuGet packages

The following libraries from this repository are available as NuGet packages:

|Library |Package|
|--------|:-----:|
|UXC.Core.Data|[![UXC.Core.Data package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/905a1e2c-1aff-45b3-bc72-dba43be0a133/_apis/public/Packaging/Feeds/990007cf-a847-406c-9fa5-dec22ee2ccdc/Packages/a1c8a100-a144-4e39-83a3-6286c3f32c8b/Badge)](https://dev.azure.com/uxifiit/Packages/_packaging?_a=package&feed=990007cf-a847-406c-9fa5-dec22ee2ccdc&package=a1c8a100-a144-4e39-83a3-6286c3f32c8b&preferRelease=true)|
|UXC.Core.Data.Compatibility.GazeToolkit|[![UXC.Core.Data.Compatibility.GazeToolkit package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/905a1e2c-1aff-45b3-bc72-dba43be0a133/_apis/public/Packaging/Feeds/990007cf-a847-406c-9fa5-dec22ee2ccdc/Packages/1d7b0c19-300c-4a8e-8e92-516b2f32d77e/Badge)](https://dev.azure.com/uxifiit/Packages/_packaging?_a=package&feed=990007cf-a847-406c-9fa5-dec22ee2ccdc&package=1d7b0c19-300c-4a8e-8e92-516b2f32d77e&preferRelease=true)|
|UXC.Core.Data.Serialization|[![UXC.Core.Data.Serialization package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/905a1e2c-1aff-45b3-bc72-dba43be0a133/_apis/public/Packaging/Feeds/990007cf-a847-406c-9fa5-dec22ee2ccdc/Packages/edac3504-808d-471c-a9d7-36c49459c51d/Badge)](https://dev.azure.com/uxifiit/Packages/_packaging?_a=package&feed=990007cf-a847-406c-9fa5-dec22ee2ccdc&package=edac3504-808d-471c-a9d7-36c49459c51d&preferRelease=true)|
|UXC.Core.Interfaces|[![UXC.Core.Interfaces package in Public feed in Azure Artifacts](https://feeds.dev.azure.com/uxifiit/905a1e2c-1aff-45b3-bc72-dba43be0a133/_apis/public/Packaging/Feeds/990007cf-a847-406c-9fa5-dec22ee2ccdc/Packages/dab1b461-9dad-4841-a44a-7138b37574f6/Badge)](https://dev.azure.com/uxifiit/Packages/_packaging?_a=package&feed=990007cf-a847-406c-9fa5-dec22ee2ccdc&package=dab1b461-9dad-4841-a44a-7138b37574f6&preferRelease=true)|

These NuGet packages are available in the public Azure DevOps artifacts repository for all UXIsk packages:
```
https://pkgs.dev.azure.com/uxifiit/Packages/_packaging/Public/nuget/v3/index.json
```

### Add UXIsk Packages to package sources
First, add a new package source. Choose the way that fits you the best:
* Add new package source in [Visual Studio settings](https://docs.microsoft.com/en-us/azure/devops/artifacts/nuget/consume?view=azure-devops).
* Add new package source from command line:
```
nuget source Add -Name "UXIsk Packages" -Source "https://pkgs.dev.azure.com/uxifiit/Packages/_packaging/Public/nuget/v3/index.json"
```
* Create or edit `NuGet.config` file in your project's solution directory where you specify this package source:
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="UXIsk Packages" value="https://pkgs.dev.azure.com/uxifiit/Packages/_packaging/Public/nuget/v3/index.json" />
    <!-- other package sources -->
  </packageSources>
  <disabledPackageSources />
</configuration>
```

### Install packages

Use the Visual Studio "Manage NuGet Packages..." window or use the Package Manager Console:
```
PM> Install-Package UXC.Core.Data
```
```
PM> Install-Package UXC.Core.Data.Compatibility.GazeToolkit
```
```
PM> Install-Package UXC.Core.Data.Serialization
```
```
PM> Install-Package UXC.Core.Interfaces
```



## Contributing

Use [Issues](issues) to request features, report bugs, or discuss ideas.


## Dependencies

* [UXI.Libs](https://github.com/uxifiit/UXI.Libs)
* [UXI.GazeToolkit](https://github.com/uxifiit/UXI.GazeToolkit/)
* [Tobii.Research](https://www.nuget.org/packages/Tobii.Research.x86/)
* [Rx.NET](https://github.com/Reactive-Extensions/Rx.NET)
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* [Ninject](https://github.com/ninject/Ninject)
* [ASP.NET SignalR](https://www.asp.net/signalr)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [Stateless](https://github.com/dotnet-state-machine/stateless)
* [CommandLineParser](https://github.com/commandlineparser/commandline)
* and others


## Copyright

Copyright (c) 2018 The UXC Authors.

See the [AUTHORS.txt](AUTHORS.txt) file for a complete list of The UXC Authors.


## License

UXC is licensed under the terms of the GNU General Public License version 3 only as published by the [Free Software Foundation](https://www.fsf.org/) - see the [COPYING.txt](COPYING.txt) file for details.

This repository contains additional separate projects that are components of the UXC application. 
All projects in this repository are licensed under the same terms of the GNU GPL v3 only unless an explicit license is located in the project's directory. License located in the project's directory takes precedence over the license of the whole repository.

Following are the projects explicitly licensed under the terms of the GNU LGPL v3 only:
* `src/core/`
  + [UXC.Core.Interfaces](https://github.com/uxifiit/UXC/tree/master/src/core/UXC.Core.Interfaces)
  + [UXC.Core.Data](https://github.com/uxifiit/UXC/tree/master/src/core/UXC.Core.Data)
  + [UXC.Core.Data.Compatibility.GazeToolkit](https://github.com/uxifiit/UXC/tree/master/src/core/UXC.Core.Data.Compatibility.GazeToolkit)
  + [UXC.Core.Data.Serialization](https://github.com/uxifiit/UXC/tree/master/src/core/UXC.Core.Data.Serialization)


## Contacts

* UXIsk 
  * User eXperience and Interaction Research Center, Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava
  * Web: https://www.uxi.sk/
* Martin Konôpka
  * E-mail: martin (underscore) konopka (at) stuba (dot) sk


## Publications

Bielikova, M., Konopka, M., Simko, J., Moro, R., Tvarozek, J., Hlavac, P., Kuric, E. (2018). Eye-tracking en masse: Group user studies,
lab infrastructure, and practices. *Journal of Eye Movement Research, 11(3)*, Article No. 6. DOI: http://dx.doi.org/10.16910/jemr.11.3.6
