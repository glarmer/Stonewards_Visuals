# Stonewards Visuals

A standalone BepInEx 5 mod project ported from PEAK Visuals for Stonewards.

## Requirements

- Stonewards installed through Steam
- BepInEx 5 installed in the Stonewards directory
- .NET SDK 8 or newer for building
- An NVIDIA RTX GPU and Direct3D 11/12 for DLSS

The project defaults to:

`~/.local/share/Steam/steamapps/common/Stonewards`

Override `StonewardsGameRootDir` in `Config.Build.user.props` if your installation is elsewhere.

## Build

Open `Stonewards_Visuals.sln` in Rider, or run:

```sh
dotnet build Stonewards_Visuals.sln
```

The output is under `artifacts/bin/Stonewards_Visuals/`. Copy the managed mod DLL and the two NVIDIA native DLLs into `Stonewards/BepInEx/plugins/Stonewards_Visuals/`.

Press F11 in game to open the settings overlay. The key can be changed in the generated BepInEx config file.
