# Final-Year-Project-2022
Overall repository for various sections of my Project
# How to install this project, THE COMPLETE GUIDE
## Prerequisites
You will need a computer, a VR headset (that is compatible with Steam - most are) and a Nintendo Wii Fit Balance Board. 

The computer will need to meet the requirements to run your VR headset. To ensure that your computer can do this, simply search your VR headset name + "Specs" or "requirements" and you should find a list of components you will need. Usually this will be a graphics card and CPU/RAM (memory) requirement.

![image](https://user-images.githubusercontent.com/26506402/161072772-a35f6a29-bc5c-42d6-a199-154fcb16ec27.png)

This system has been tested on an Oculus Quest 2 and a Pimax 5k Headset, using an RTX 3060ti NVIDIA Graphics card and 8gb RAM.

## Setting up VR
You will need to install Steam on the machine, create an account on Steam, and install SteamVR on the machine. This enables almost any VR to communicate with Unity.

You will also need to install the relevant drivers/software that enables your VR device to operate on a computer. In the case of Oculus, this was the "Oculus" app on PC for example. For the Pimax, simply install PiTool from the Pimax website. For every VR system, there should be instructions with the manual or online for this.

You may need to run the relevant software in order to activate the VR. For example, with the Pimax 5k, you will need to run PiTool *every time you intend to use the Pimax*. In the case of the Pimax, this tool will also assist you on setting up your VR environment. If your software does not set up your environment, then SteamVR can also set this up for you. 

### Base Station

To set up the Virtual Environment, if your VR does not have position tracking (most do not, however Oculus Quest 2 does - so you can skip this step) you will need to set up "Base Stations". These track your VR headset position. Base stations are often cross-compatible. The Pimax 5k supports base stations from HTC and Index, among others. 

The following instructions are for the HTC Vive Base Stations, though any other stations will behave similarly. For more information on how other stations work, check the producers' website. Youtube is your friend. 

To set up the HTC Vive Base Stations, fix them on opposite corners in the space you want to create your Virtual Environment in. Make sure they are as high as possible, and that they have unobstructed vision of each other, and angle them slightly towards the ground for optimal performance. Think of these as cameras, you want to make sure you cover the whole area where your VR headset might be with these. Once you are happy with their position, wire them up to the mains. Press the button on the back of each Base Station until one has the letter "A" and the other "b". The light on the top is green for both, then they are successfuly set up. If for any reason you are not able to make these lights turn green (they turn green when the stations can see each other) simply switch the base station marked "A" to "c", and connect both stations using the long connection wire. For more information see this check [this video out](https://www.youtube.com/watch?v=fV--q0HcDU4).

HTC Vive Base Stations do not require any physical connection to the PC. 

### Setting up your space

You should now have your drivers installed, and any base stations set up too. To ensure your base stations are correctly enabled, run SteamVR. A small window should open up showing which devices are connected. 

Hopefully it should look like this: 

![image](https://user-images.githubusercontent.com/26506402/161076942-98890fb8-6b5f-4459-8056-bc0f40cc7326.png)

For the Pimax 5k, the PiTool should also show a similar silhouette of the base stations and headset. The PiTool also provides troubleshooting advice and error messages, for example if the headset is not being tracked by the stations the PiTool will notify you. By selecting "Guide" on the PiTool and following the on-screen instructions you can correctly callibrate your VR environment.

As mentioned before, if you are not using the Pimax and your VR headset does not provide an environment set-up option, SteamVR offers its own. By clicking the 3 bars on the top left of the SteamVR window, you should then be able to select "Room Setup" at the top of the dropdown list. From here you can follow the onscreen instructions to set up your environment

## Software requirements

You will also need Unity installed to run the VR portion of this project, and will need Python installed to run the data analysis and graphing portion of this project.

### Unity 

### Python
=======
# Wii Board bluetooth to C# socket connector for Windows

Wii Board bluetooth to C# socket connector is a C# application that enables users to connect to their Wii board, get sensor readings, record sensor latency, as well as provide a C# UDP client socket interface for other softwares to connect to. This tool will send live updates over the socket, as well as allowing a toggleable timestamp message to measure latency.

## Installation

Clone the main branch for full build, else just download the .exe file.

## Usage
### Connecting Wii Board
To connect a wii board, first you need a bluetooth adapter on your device. If you have previously connected the board to your pc (using this guide), you must now unpair the board. Next, go to Control Panel -> Search "Add bluetooth device" and select the option to add a bluetooth device. From here you should be able to see your Wii Board as an option. Select it, and you will be prompted with a pin entry screen. Ignore this and press Next. This should connect the wiiboard and enable it to be found by this tool. 

*N.B. Using Settings -> Add bluetooth device may not work as this interface requires pin entry*


### Tool interface
Once connected to a wiiboard, the tool offers multiple options, each accessible by keypress:

```d``` - Toggles live sensor readings
```Escape``` - Exits program
```t``` - Toggles sending timestamps over socket- for measuring latency in sockets
```p``` - Allows client to change Port number
```Spacebar``` - Records time of spacebar press, adds to list of spacebar press timestamps (for recording sensor delay)
```s``` - Displays average time between readings and standard deviation
```r``` - Toggles recoding sensor readings. When pressed again, stops recording sensors then produces a "data.csv" file in the following format: 
```
Raw Top Left, [reading 1], [reading 2] ...
Raw Top Right, [reading 1], [reading 2] ...
Raw Bottom Left, [reading 1], [reading 2] ...
Raw Bottom Right, [reading 1], [reading 2] ...
Time, [time at reading 1], [time at reading 2] ...
Spacebar clicks, [spacebar click time 1], [spacebar click time 2] ...
```
From this information it is possible to obtain latency between sensor readings and real world events by (gently) tapping a keyboard onto the board so that the spacebar key is pressed. 
### Socket interface
Included with this is a UDPSocket.cs file which can be used to create a Server Socket that connects to this client. Either by changing the source code or by using the ```p``` option, the Client port can be changed to match a server.
The Client sends information in the following format:
```
[sensor_name]:[value]
```
[sensor_name] corresponds to one of the 4 wiiboard sensors, namely:
"rTL" (raw Top Left), "rTR" (raw Top Right), "rBL" (raw Bottom Left), "rBR" (raw Bottom Right)
To correctly orient the board, ensure the Wii symbol is at the front, and the "A" button is facing the bottom.
When the ```t``` option is set to send timestamps, the client also sends the following:
```
timestamp:[time stamp computed using DateTime.Now.TimeOfDay.TotalMilliseconds]
```
## Troubleshooting
### Wiiboard not connecting to device
If the wiiboard is failing to connect to the device, ensure that
1) The device has a functioning bluetooth adapter
2) The Wiiboard is not paired with the device
3) Ensure you are using the Control Panel method, not by connecting via Settings
 
### Wiiboard connecting to PC but not tool (tool says it cannot find any Wii boards)
Turn the Wiiboard off, unpair the Wiiboard from the device, then re-attempt to connect the Wiiboard.
### Wiiboard sensor values are frozen
This is usually indication that the Wiiboard needs to change batteries.
## Implementation Justification
This project was created using the C# library "Wiimote". This offers an excellent interface for connecting and receiving data from Wii remotes. 
The original implementation used a .NET Timer with the smallest possible interval to scan the Wiiboard sensors. This has now been majorly improved using a thread, running a ```while (true)``` loop to ensure the fastest possible scanning of sensors. This meant that average sensor scan rates went down from 16ms to 0.2ms. In turn this increased sensor impact capture rate from 50% (sensing sudden brief impacts only 50% of the time) to 80% capture rate, and also decreased the time delay from impact to sensor readings from 20ms (with 18ms standard deviation) to 10ms (8ms standard deviation).
## License+-
[MIT](https://choosealicense.com/licenses/mit/)
