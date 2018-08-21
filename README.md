# <img src="docs/logo.png" height="28" /> UXC 

###### Client application of the UXI Group Studies infrastructure.
UXC is designed for creating and recording user studies with eye tracking. UXC client and UXR web management applications form the UXI Group Studies infrastructure for conducting group eye tracking studies. This project is developed at [User eXperience and Interaction Research Center](https://www.uxi.sk/) of [Faculty of Informatics of Information Technologies, Slovak University of Technology in Bratislava](http://fiit.stuba.sk/).

For more information about the infrastructure, group studies and our lab, see the [Publications](#publications) section below.

See [UXC project Wiki](https://github.com/uxifiit/UXC/wiki) for documentation, examples and guides.



![UXC](/docs/uxc.png)

## Main features 
* Controlling devices for recording during the experiment session.
* Experiment session recording with stimuli timeline playback
* Connection with the **[UXR management application](https://github.com/uxifiit/UXR/)** for:
  + Remote start and control of the session recording,
  + Upload recording data on session finish
* Integration options for 3rd party applications used in the experiment, e.g., a web or desktop application through locally hosted REST API and web sockets, providing them with:
  + Real-time access to eye tracking and other session recording data 
  + Session recording control, i.e., to start/stop recording, query status, or control stimuli timeline playback
  + Writing custom events

### Session control

Session recording can be started and controlled in 3 ways:
* Remotely by the study conductor through the UXR management application,
* Locally, using the application user interface, or
* Through the REST and web socket API by a 3rd party application.

### Supported devices

These devices and event sources are supported for data collection with UXC:

* Tobii Pro eye trackers - using the [Tobii Pro SDK](http://developer.tobiipro.com/) - Tobii X2-60 and TX300 were tested,
* Webcam video and audio recording - using the [FFmpeg](https://www.ffmpeg.org/) binary,
* Screencast video recording - using the [UScreenCapture](http://www.umediaserver.net/umediaserver/download.html) utility and FFmpeg binary,
* Keyboard and mouse events logging - using the Windows API hooks,
* External events logging - using the locally hosted REST API and websockets with [ASP.NET SignalR](https://www.asp.net/signalr).

### Stimuli timeline

These types of stimuli are provided for playback during the session recording:

* Eye tracker calibration with customizable calibration plans
* Eye tracker validation
* Instructions
* Questionary with write or choose answer questions
* Show desktop
* Launch program
* Introduction into experiment


## Solution structure

Source code folder `src` structure:

* apps - the main app project.
* core - core libraries of the application.
* devices - libraries implementing communication with recording devices.
* plugins - optional plugins for the application



## Contributing

Use [Issues](issues) to request features, report bugs, or discuss ideas.

## Dependencies

* [Tobii.Research](https://www.nuget.org/packages/Tobii.Research.x86/)
* [UXI.Libs](https://github.com/uxifiit/UXI.Libs)
* [UXI.GazeToolkit](https://github.com/uxifiit/UXI.GazeToolkit/)
* [Rx.NET](https://github.com/Reactive-Extensions/Rx.NET)
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
* [Ninject](https://github.com/ninject/Ninject)
* [ASP.NET SignalR](https://www.asp.net/signalr)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [Stateless](https://github.com/dotnet-state-machine/stateless)
* [CommandLineParser](https://github.com/commandlineparser/commandline)
* and others

## Authors

* Martin Konopka - [@martinkonopka](https://github.com/martinkonopka)

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details

## Contacts

* UXIsk 
  * User eXperience and Interaction Research Center, Faculty of Informatics and Information Technologies, Slovak University of Technology in Bratislava
  * Web: https://www.uxi.sk/
* Martin Konopka
  * E-mail: martin (underscore) konopka (at) stuba (dot) sk

## Publications

Bielikova, M., Konopka, M., Simko, J., Moro, R., Tvarozek, J., Hlavac, P., Demcak, P. (2018). Eye-tracking en masse: Group user studies,
lab infrastructure, and practices. *Journal of Eye Movement Research, 11(3)*, Article No. 6. DOI: http://dx.doi.org/10.16910/jemr.11.3.6
