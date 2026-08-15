# StoneWards Visuals

Do you wish you could make rock look even more ... *stone*? Well have I got the solution for you <br>

Introducing, **"StoneWards Visuals"**, a client-side mod for StoneWards which unlocks better graphics options for StoneWards! 

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Q5Q7IFRUH)

- Did you know? You can get help from myself via my [Discord server](https://discord.gg/cjzWT37abR)!

## Features:
- Adds the ability to run at higher than native render scales (supersampling)
- Adds antialiasing options including the option to disable TAA and enable MSAA
- Allows for granular choices over resolution scale, shadow settings, upscaling and LOD quality
- Adds DLSS with configurable presets!! **You need to install the dependency UpscaleLib for this functionality**

The default configuration aims to be a balance between high quality, clarity and performance. 

To this end in the default configuration:
- TAA is disabled (to reduce blur) and it is replaced with MSAA + SMAA.
- Shadowmap Resolution is set to 4096, shadow distance to 300 (100 higher than default "High"), and shadow cascades are set to 4.
- Anisotropic Filtering is set to ON, this option makes textures render more correctly at angles, which increases clarity.
- LOD Quality is increased to 2.5
- Render Scale is left at 1.0, increasing it even a little bit (e.g. 1.2x) can improve distant objects a lot but it does effect framerate a lot!
- **This can be configured entirely by you in game using the F11 menu, equally, the main menu is rendered in real time so you can test it there too**

## Configuration
- You can open the in game mod menu with F11 - all options can be changed here.
- The mod menu's keybind can be changed both in the menu and from the configuration file
- Configuration can be found at: (steam directory)\StoneWards\BepInEx\config\StoneWards_Visuals.cfg
- Every single option can be configured and is explained in detail within the configuration file.
- These options will effect performance in some way, high values of Render Scale in particular can reduce FPS.
- The settings in this mod will override the settings from the StoneWards Graphics settings.

## Commissions
Did you know I take commissions? Contact me on discord @glarmer if you'd like to discuss! Alternatively reach out to me via my [Discord server](https://discord.gg/cjzWT37abR). Note: All commissions must go through PayPal for the safety of you as a buyer and me as a seller.

## Manual Installation
This is a clientside mod! Automatic installation via Gale or R2ModMan is recommended!

1.) Download Bepinex from [here](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.3/BepInEx_win_x64_5.4.23.3.zip) <br>
2.) Extract the contents of that zip into your game directory (default: C:\Program Files (x86)\Steam\steamapps\common\StoneWards) resulting in a folder that has the following files: <br>
3.) Start the game and close it again, this does the first time set up for Bepinex. <br>
- Linux users: set the launch option `WINEDLLOVERRIDES="winhttp=n,b" %command%` before running the game.
  
4.) Navigate to ...\StoneWards\BepInEx\plugins, dlls from "Manual Download" on Thunderstore into this folder. <br>
5.) Run the game <br>

## Important
- DO NOT UNDER ANY CIRCUMSTANCE COMPLAIN ABOUT BUGS TO THE DEVELOPERS WHILE USING MODS. UNINSTALL MODS IF YOU ENCOUNTER BUGS AND THEN REPORT THEM IF THEY ARE STILL PRESENT.
- Please search the StoneWards Steam discussions if you face bugs as they are possibly caused by the game and not the mod. If you continue to experience issues then please reach out via the GitHub issues.
- These options will effect performance in some way, high values of Render Scale in particular can reduce FPS.

## Help
- You can find your StoneWards folder by **right clicking the game in your Steam library** then > **Manage** > **Browse Local Files**
- You can get help from myself via my [Discord server](https://discord.gg/cjzWT37abR)!
