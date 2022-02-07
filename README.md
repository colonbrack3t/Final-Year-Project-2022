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
