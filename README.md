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

### Wii Board
The Wii board gameobject contains all relevant scripts needed. These scripts are "Balance Board Sensor" and "Estimate Height".

#### Balance Board Sensor class

This class operates the connection to the balance board [C# client Socket tool](https://github.com/colonbrack3t/wiiboard-unity-scripts/tree/main/Wii-Balanceboard-server). We connect to the client using the UDPSocketUnity class, which inherits from the UDPSocket class (the same one that is used in the client tool). The child class overrides the Recieve function to synchronously update the Balance Board class sensor values. 

**N.B ensure the port matches the client port**

This class also handles recording various elements of the setup. Namely:
1) the Wiiboard sensor readings
2) the Wiiboard sensor + Headset height (To use this to measure delay, one can - gently - tap the headset on the board, and compare minimum height of headset vs sensor readings)
3) the Wiiboard sensor + mouseclicks (To use this to measure delay, one can hover the mouse over the mouse record button and press it on the board such that it produces a click, and compare the mosue click times vs sensor readings)
4) Latency - records latency between sockets (requires "latency mode" to be active on tool)

Each of these recordings produce a "data.csv" file after being toggled off. This data.csv is a comma seperated database, where each line represents an element that was recorded. 

**N.B while it is possible to toggle multiple modes at the same time, however when toggling off, each recording while overwrite the data.csv file. Therefore one should rename/move the data.csv each time after toggling off each record mode.**

This class also performs a toggleable weight estimation, by recording the sensor readings on all 4 weights. When toggled on, it begins computing the average weight, when toggled off the weight attribute holds the final average weight estimation. The user can be asked to stand still for a few seconds in order to gain an accurate average. 

#### Estimate Height
This class is mostly deprecated. Originally the user would be asked to lean back and forth in order to estimate where their pivot point is. This has been removed in favour of the user setting where their feet are using the controller. The user can press the primary button on the right to set the wiiboard object to their right controller. They can press right trigger to lock in the change.
#### Swaying Particles
There are different attempts at moving objects in convincing ways. Swaycube.cs + GenerateParticles.cs is the current implementation, which had the best results and has been developed past a prototype.
##### Sway Cube & Generate Particles
Both these classes are implemented onto GameObjects called "Tunnel". Generate Particles generates particles in either a spherical or cylindrical shape, with every feature of either  object fully customizable from the inspector menu. These are capable of generating thousands of small sphere objects which are minimally expensive to compute. Swaycube is responsible for movement of these tunnels. Swaycube uses the Wiiboard weight and sensor readings as well as the z position of the headset to set the z position of its object. Weight is used to normalise the sensor readings.

##### Other attempts at moving objects
The "Accentuate" scripts aimed to exagerate the movement of the headset by over-rotating the camera. This idea was quickly scrapped due to no obvious way to incorporate the wiiboard/force plate, and the potential for nausea.

The Moving UI scripts aimed to utilise 2D images to effectively directly manipulate pixels on the VR headset to reduce computational effort of generating 3d objects. However this actually appeared to use more computational power to make convincing smooth movement, especially as the UI camera uses an orthographic camera.

#### Stroop test 
The stroop test script is simple, upon being prompted the script sets a text field with a random word from a list of colours and sets the font color to a random preset list of colours. It also sets an image element to the chosen colour, which is only visible on the computer, such that the tester can see if the user got the right answer. 
#### App Manager
This operates similarly to a Game Manager,  acting as a central point that conducts what is being done when. This is implemented using a switch clause on the current stage.
##### Stage enum
This simple enum holds every stage progession of the project, such that we can easily decide what to do in the Update() function based on the current stage.

## Troubleshooting
