# FYP Unity Components
All components of Unity needed to both connect a device to a wiiboard, and to create and run the FYP simulation.

- [FYP Unity Components](#fyp-unity-components)
- [Tutorial on how it works](#tutorial-on-how-it-works)
- [Installation](#installation)
  * [Full build](#full-build)
  * [Build from Source](#build-from-source)
- [Usage](#usage)
  * [Running the project on VR](#running-the-project-on-vr)
- [Using and understanding the source code](#using-and-understanding-the-source-code)
  * [Wii Board](#wii-board)
    + [Balance Board Sensor class](#balance-board-sensor-class)
    + [Particles and Changing Sway](#particles-and-changing-sway)
      - [Generate Particles](#generate-particles)
      - [position_and_rotation_manager](#position-and-rotation-manager)
- [Troubleshooting](#troubleshooting)
  * [Port error on runtime](#port-error-on-runtime)
  * [Wii board connection issues / Wii board tool issues](#wii-board-connection-issues---wii-board-tool-issues)
- [License+-](#license--)

# Tutorial on how it works
This project is used to measure the effect of visual sway on balance. When the program is run, a user will be in the middle of an immersive field of particles. The user is free to move and turn their head, and they will experience usual 1:1 actions in the VR world - as is normal in VR. When Begin Trial is pressed, for a number of seconds (defined by the "Control Stage" time parameter), nothing will happen and the VR user will see no change in how their actions are reflected in VR world. Then, for a number of seconds equal to the "Test Stage" parameter, the user will exprience a 1 : (1 + sensitivity parameter) visual sway. That is, if the paramter = 1, then the user experiences 1 : 2 sway. So in that case, when the user turns 90 degrees they see the world spin 180 degrees instead. If they move 10cm it looks like they moved 20cm. Then once this period has passed, the user is restored to 1 : 1 for the duration of the Aftermath stage. Once the aftermath stage is finished, a csv file will be created with all the recorded data and a final "Pause stage" timer will count down, where the user is expected to take off their headset in preparation for the next trial. If Begin Trial is pressed again, the trial will begin again, this time taking the next sensitivity from the list. 

If the user presses Reset Trial at any point, the trial will end, and the next sensitivity will be set to the first on the list again. 

If the user presses Recentre the VR user will be teleported back to the centre of the scene, useful when multiple trials are played back to back. 
![image](https://user-images.githubusercontent.com/26506402/163415731-aa4c0850-f738-43e4-9d65-19ec6c69a50a.png)
# Installation
## Full build
Download thre binary from the releases - which allows you to set sensitivities lists and change stage length times, but nothing else.


## Build from Source
First you need to create a Unity Project v2021.2.10 (ideally using Universal pipeline). Install XR SDKs and XR tools from the package manager, and enable XR from the preferences menu. For more information check this [video](https://www.youtube.com/watch?v=yxMzAw2Sg5w)

Once you have a functioning Unity XR project, moving this repository into the Assets folder should grant everything you need to run the simulation.

From this point you should be able to load the various scenes directly. Should they fail for any reason, or if you want to construct your own scene, refering to this documentation should aid in the reassembly of the scenes. In some cases, if the XR packages have changed, you may have to replace the XR objects in the scene with the up-to-date version.

# Usage
## Running the project on VR
Whether you have built your own binary or are running the executable found in the releases, in order to run the simulation you must run the application from the VR console. To do this with an Oculus Quest, simply connect to your device via Link, and run the executable / Unity project. The process is similar for other VR consoles, typically making use of SteamVR. 
The device which the project is running on will display the correct answers to each stroop test presented to the user. 
# Using and understanding the source code

## Wii Board
The Wii board gameobject contains all relevant scripts needed. These scripts are "Balance Board Sensor" and "Estimate Height".

### Balance Board Sensor class

This class operates the connection to the balance board [C# Wiiboard client Socket tool](https://github.com/colonbrack3t/Wiiboard-Socket-Tool). We connect to the client using the UDPSocketUnity class, which inherits from the UDPSocket class (the same one that is used in the client tool). The child class overrides the Recieve function to synchronously update the Balance Board class sensor values. 

**N.B ensure the port matches the client port**

This class also handles the Wiiboard sensor readings

### Particles and Changing Sway
There are different attempts at moving objects in convincing ways. PendulumMovement.cs + GenerateParticles.cs is the current implementation, which had the best results and has been developed past a prototype.
#### Generate Particles
Generate Partilces is implemented onto GameObjects called "Tunnel". Generate Particles generates particles in either a spherical, cylindrical or cloud shape, with every feature of the object fully customizable from the inspector menu. These are capable of generating thousands of small sphere objects which are minimally expensive to compute.
#### position_and_rotation_manager
This manages the position and rotation of the in-game camera. It uses the VR head set input positions. The code is reasonably simple. Have an empty game object which tracks the VR headset position and sets its own position to the headset position (Tracked Pose Driver). Refer this to get a frame by frame change in orientation and position. Augment these values by a constant modifier, and apply to the camera.
# Troubleshooting
If you have any problems or questions that are not addressed or solved by something mentioned in this documentation, do not hesitate to contact me at ludoattuoni@gmail.com
## Port error on runtime
If you get an error stating multiple servers cannot be opened on the same port, simply changing the port defined in the BalanceBoard.Start() function should resolve this problem (don't forget to also switch ports on the Wiiboard tool) 

## Wii board connection issues / Wii board tool issues
Please refer to the [C# Socket Wiiboard tool documentation](https://colonbrack3t.github.io/Wiiboard-Socket-Tool/)

# License+-
[MIT](https://choosealicense.com/licenses/mit/)

