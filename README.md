# Final-Year-Project-2022
Overall repository for various sections of my Project

To access the sections of my project, simply switch between the branches. The releases section also has any available binaries.

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
