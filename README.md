# FYP Unity Components
All components of Unity needed to both connect a device to a wiiboard, and to create and run the FYP simulation.

## Installation
### Full build
Download thre binary from the releases 
### Build from Source
First you need to create a Unity Project v2021.2.10 (ideally using Universal pipeline). Install XR SDKs and XR tools from the package manager, and enable XR from the preferences menu. For more information check this [video](https://www.youtube.com/watch?v=yxMzAw2Sg5w)

Once you have a functioning Unity XR project, moving this repository into the Assets folder should grant everything you need to run the simulation.


## Usage
### Running the project on VR
Whether you have built your own binary or are running the executable found in the releases, in order to run the simulation you must run the application from the VR console. To do this with an Oculus Quest, simply connect to your device via Link, and run the executable / Unity project. The process is similar for other VR consoles, typically making use of SteamVR. 
The device which the project is running on will display the correct answers to each stroop test presented to the user. 

### Using and understanding the souce code
The project can be seen as 3 key components: Wii Board, Sway mechanic and Stroop test.

## Wii Board
The Wii board gameobject contains all relevant scripts needed. These scripts are "Balance Board Sensor" and "Estimate Height".

Balance Board Sensor class

This class operates the connection to the balance board [C# client Socket tool](https://github.com/colonbrack3t/wiiboard-unity-scripts/tree/main/Wii-Balanceboard-server). We connect to the client using the UDPSocketUnity class, which inherits from the UDPSocket class (the same one that is used in the client tool). The child class overrides the Recieve function to synchronously update the Balance Board class sensor values. 

*N.B ensure the port matches the client port*

This class also handles recording various elements of the setup. Namely:
1) the Wiiboard sensor readings
2) the Wiiboard sensor + Headset height (To use this to measure delay, one can - gently - tap the headset on the board, and compare minimum height of headset vs sensor readings)
3) the Wiiboard sensor + mouseclicks (To use this to measure delay, one can hover the mouse over the mouse record button and press it on the board such that it produces a click, and compare the mosue click times vs sensor readings)
4) Latency - records latency between sockets (requires "latency mode" to be active on tool)

Each of these recordings produce a "data.csv" file after being toggled off. This data.csv is a comma seperated database, where each line represents an element that was recorded. 

*N.B while it is possible to toggle multiple modes at the same time, however when toggling off, each recording while overwrite the data.csv file. Therefore one should rename/move the data.csv each time after toggling off each record mode.*



## Troubleshooting
