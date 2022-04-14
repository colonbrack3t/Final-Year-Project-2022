
# Final-Year-Project-2022
created by Ludo Attuoni


Overall repository for various sections of my Project

To access the sections of my project, simply switch between the branches. The releases section also has any available binaries.

- [Final-Year-Project-2022](#final-year-project-2022)
- [How to install this project, THE COMPLETE GUIDE](#how-to-install-this-project--the-complete-guide)
  * [Prerequisites](#prerequisites)
  * [Setting up VR](#setting-up-vr)
    + [Base Station](#base-station)
    + [Setting up your space](#setting-up-your-space)
  * [Install current solution](#install-current-solution)
  * [Software requirements](#software-requirements)
    + [Unity](#unity)
    + [Python](#python)
  * [Building the Unity project](#building-the-unity-project)
- [Troubleshooting](#troubleshooting)


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

## Install current solution
After you have set up the prequisities, you can now choose whether you want to run this project as is, or install it from source (which offers much more customization). If all you want to do is run the trials and change sensitivities and the amount of time for each stage, you can install the current solution. 

To do so, simply navigate to the [releases section of this github](https://github.com/colonbrack3t/Final-Year-Project-2022/releases). Download both Wiiboard.client.zip and Final.Year.Project.zip. From there, you can run the executables found in both zip files (the Unity executable is found within Build, entitled "Ludo VR Project", in Wii Board the executable is called "Wii_Balanceboard_client"). Follow the readme documentation found on [the Wii Board script github page](https://github.com/colonbrack3t/Final-Year-Project-2022/tree/Wii-Board-Socket#readme) to connect the Wii Board. NOTE THE UNITY PROJECT IS RUNNING ON PORT 27338! 

Carry on with this guide if you would like to build this project as a Unity project, which would enable you to change more aspects of the program as well as the code (for example if you would like to change the number of particles generated). You do not need to know how to program in C# to edit some features such as particles- the project has been made with the intention to be as easy to use as possible. 

## Software requirements

You will need Unity installed to run the VR portion of this project, and will need Python installed to run the data analysis and graphing portion of this project.

### Unity 
You will need to install this from the [Unity website](https://unity.com/download). You wil also need to create a Unity account. 

### Python
This is only neccesary if you want to use the graphing part of this project, you can ignore this if you have your own methods of data analysis. The project creates csv files that can be read using most data analysis tools. To use the Python section you will need to [install python](https://realpython.com/installing-python/). You will also need to run the following command to install some needed packages.
```
pip install numpy matplotlib
```
## Building the Unity project
Assuming you have installed all relevant software, you should now have everything you need to build this project. Create a new Unity 3D project and unzip the sourcecode from this [git repository](https://github.com/colonbrack3t/Final-Year-Project-2022/tree/Unity-Components) on the Unity-Components branch in, or clone this repository, into the Assets folder. You will then have to open the package manager window (windows > Package manager) and install the XR Interaction Toolkit. You will also need to go into File>Project Settings>XR plug-in management and enable OpenVR. Refer to [this video](https://www.youtube.com/watch?v=yxMzAw2Sg5w) for more help.   

More help for this section is also provided in the [documentation for this code](https://github.com/colonbrack3t/Final-Year-Project-2022/tree/Unity-Components#readme)
## Connecting the WiiBoard
Follow the readme documentation found on [the Wii Board script github page](https://github.com/colonbrack3t/Final-Year-Project-2022/tree/Wii-Board-Socket#readme) to connect the Wii Board. NOTE THE UNITY PROJECT IS RUNNING ON PORT 27338! This can be changed in the Wiibalanceboard class.

# Troubleshooting
If you have any problems or questions that are not addressed or solved by something mentioned in this documentation, do not hesitate to contact me at ludoattuoni@gmail.com
## License+-
[MIT](https://choosealicense.com/licenses/mit/)
