# Parkitect StatMaster

A Parkitect Game Modification which offers the ability to:
* record / handle
* save / load
* manage / export

... all statistics of your game sessions.

## Compatibility
+ Please use pre-alpha 4 (or later) parks to get full park statistics
  - old parks (pre-alpha 3, savegames) can not have own statistics currently
  - as long as Parkitect does not generate a Park-GUID therefor
+ AutoSave Mod related, current problems with regular save actions after AutoSave mod actions
  - https://parkitectnexus.com/assets/472f0d493e/autosave#comment-95881f2823

## Ingame Settings

**Shortcut**: LeftCtrl + F12

+ Update Game Data
  - First session start / last session end time
  - Start time of every game session
+ Update Park Data (depends on Game Data)
  - First session start / last session end time
  - Start time of every park session
  - Last internal park time
  - Used park names
  - Used file names
+ Update Park Session Data (depends on Park Data)
  - Start time of park session
  - Last internal park time
  - Used park names
  - Loaded file name
  - Further used file names
+ Update Park Progression Data (depends on Park Session Data)
  - Guests, employees, attractions, shops count
+ Update Interval Progression Data (every 1 - 120 seconds)
  - all other data will be updated on session start / end
  - or if a related ingame event has been occured
+ Update Further Park Progression Data (depends on Park Progression Data)
  - Money, entrance fee, cleanliness, happiness, price satisfaction
+ Update Attractions Progression Data (depends on Park Progression Data)
  - Average attraction entrance fee
+ Update Further Attractions Progression Data (depends on Attractions Progression Data)
  - Attractions opened & customers count
+ Update Shops Progression Data (depends on Park Progression Data)
  - Average shops item price
+ Update Further Shops Progression Data (depends on Shops Progression Data)
  - Shops opened & customers count
+ Update AutoSave mod data (depends on Park Data)
  - AutoSaves / QuickSaves count (for Park and Park Session)
+ Ignore AutoSave-File names (in regular park file names log)
+ Ignore QuickSave-File names (in regular park file names log)
+ Developer Mode activation
  - Set debug messages and show dev UI controls

### Contributors
* Martin 'yokumo' Kelm  @ [parkitectLab](https://github.com/parkitectLab/statMaster)
* Tim Potze @ [ikkentim](https://github.com/ikkentim/statMaster)
