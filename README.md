# CaptainShotgunModes

## Description

This mod allows you to choose between 3 firing modes for the captain's shotgun: `Normal`, `Auto` and `AutoCharge`.
With both auto modes you can just hold down the fire key and don´t have to spam it.

The modes can be selected using the number keys `1`, `2` and `3`, or cycled through using the `mouse wheel` or [DPad](https://en.wikipedia.org/wiki/D-pad).
The current mode is displayed above the primary skill icon.

## Modes

| Mode       | Key | Description | Screenshot |
|------------|-----|-------------|------------|
| Normal     |  1  | The default mode/behavior of the captain's shotgun. | ![normal](https://raw.githubusercontent.com/Vl4dimyr/CaptainShotgunModes/master/images/sc_normal.jpg)
| Auto       |  2  | Automatically fires as long as the fire key is pressed. | ![auto](https://raw.githubusercontent.com/Vl4dimyr/CaptainShotgunModes/master/images/sc_auto.jpg)
| AutoCharge |  3  | Automatically fires after charging as long as the fire key is pressed. | ![auto_charge](https://raw.githubusercontent.com/Vl4dimyr/CaptainShotgunModes/master/images/sc_auto_charge.jpg)

### Cycle through modes

| Direction | Actions                                 |
|-----------|-----------------------------------------|
| Forward   | Mouse Wheel Down, DPad Right, DPad Down |
| Backward  | Mouse Wheel Up, DPad Left, DPad Up      |

> I did add controller support because enough players wanted it! [see](https://github.com/Vl4dimyr/CaptainShotgunModes/issues/1).

> Currently this is only tested with a xbox 360 controller (xbox one should work too).

## Config

### TL;DR

Use [Risk Of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options/) for in-game settings!

![Risk Of Options Screenshot](https://raw.githubusercontent.com/Vl4dimyr/CaptainShotgunModes/master/images/risk_of_options.jpg)

### Manual Config

The config file (`\BepInEx\config\de.userstorm.captainshotgunmodes.cfg`) will be crated automatically when the mod is loaded.
You need to restart the game for changes to apply in game.

#### Example config

The example config keeps only mode selection with number keys enabled and sets the default mode to `Auto`.

```ini
## Settings file was created by plugin CaptainShotgunModes v1.1.0
## Plugin GUID: de.userstorm.captainshotgunmodes

[Settings]

## The mode that is selected on game start. Modes: Normal, Auto, AutoCharge
# Setting type: String
# Default value: Normal
DefaultMode = Auto

## When set to true modes can be selected using the number keys
# Setting type: Boolean
# Default value: true
EnableModeSelectionWithNumberKeys = true

## When set to true modes can be cycled through using the mouse wheel
# Setting type: Boolean
# Default value: true
EnableModeSelectionWithMouseWheel = false

## When set to true modes can be cycled through using the DPad (controller)
# Setting type: Boolean
# Default value: true
EnableModeSelectionWithDPad = false
```

## Manual Install

- Install [BepInEx](https://thunderstore.io/package/bbepis/BepInExPack/) and [R2API](https://thunderstore.io/package/tristanmcpherson/R2API/)
- Download the latest `CaptainShotgunModes_x.y.z.zip` [here](https://thunderstore.io/package/Vl4dimyr/CaptainShotgunModes/)
- Extract and move the `CaptainShotgunModes.dll` into the `\BepInEx\plugins` folder

## Changelog

The [Changelog](https://github.com/Vl4dimyr/CaptainShotgunModes/blob/master/CHANGELOG.md) can be found on GitHub.

## Bugs/Feedback

For bugs or feedback please use [GitHub Issues](https://github.com/Vl4dimyr/CaptainShotgunModes/issues).

## Help me out

[![Patreon](https://cdn.iconscout.com/icon/free/png-64/patreon-2752105-2284922.png)](https://www.patreon.com/vl4dimyr)

It is by no means required, but if you would like to support me head over to [my Patreon](https://www.patreon.com/vl4dimyr).
