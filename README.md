# Argonium
A sample project for VizUTM based on the Argon UTM backend. The application itself is meant for Air Traffic Controllers (ATCs) and UAS Service Suppliers (USS). It allows them to visualize and observe a digital twin of their airspace. They can freely roam around and select the digital twins to learn more information.  
<img width="2560" height="1440" alt="UTMDigitalTwinsUI" src="https://github.com/user-attachments/assets/1e304146-1aff-40b2-bc61-939db3331825" />

## Where to Start
### Explorers
Simply want to check out what's up? Download and extract the build, and run the `.exe`. Use either a controller or keyboard/mouse (See below for Controls). 

### Developers
Clone the repo and check out the project from Unity Editor. Play the project in the editor to explore.

## Features
- **Free Roam:** Fly around to any location in the world (Feature from Cesium)
- **Cache Mode:** Visualize UTM digital twins without a deployed backend (Default, data limited to Chicago)
- **Live Mode:** Connect to an Argon backend for live airspace visualization
- **Cesium OSM:** Visualize basic 3D structures in areas without Google 3D Tile coverage (Feature from Cesium)
- **UTM Digital Twins:** Visualize Geofences, Flight Declarations, and RID. (Cache Mode has no RID data or simulation)
- **Info Panel:** Hover and select an item to learn more information about it.

## Controls
Argonium supports both controller (Xbox and Xbox-based) as well as keyboard and mouse as input schemes. 
### Controller
The controller utilizes the crosshair in the center of the screen to determine what will be selected. It cannot interact with the UI in the main experience. Controller does not currently support an on-screen keyboard; Settings cannot be configured via the controller alone. 
<img width="1993" height="862" alt="ControlScheme" src="https://github.com/user-attachments/assets/ed46b663-235d-4492-b2a2-3d4101ee3695" />

### Keyboard and Mouse
With keyboard and mouse, you are free to hover over any object with the cursor and left-click to select. The crosshair can be ignored. 
- **WASD:** Move
- **Q:** Fly Down
- **E:** Fly Up
- **Right-Click and Drag:** Camera Look
- **Left-Click:** Select
- **I:** Info Panel
- **R:** Toggle RID Hitbox
- **F:** Toggle Flight Declaration Hitbox 
- **G:** Toggle Geofence Hitbox
