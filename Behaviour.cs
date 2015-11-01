using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace StatMaster
{
    class Behaviour : MonoBehaviour
    {
        private Data _data = new Data();

        private DebugNotifier _debug = new DebugNotifier();

        private uint eventsCallCount = 0;

        private uint tsSessionStart = 0;

        private bool validPark = true;

        private bool debugMode = false;

        private void Awake()
        {
            Parkitect.UI.EventManager.Instance.OnStartPlayingPark += onStartPlayingParkHandler;
            if (debugMode == false) _debug.outputActive = false;
        }

        void Start()
        {
            Parkitect.UI.EventManager.Instance.OnStartPlayingPark -= onStartPlayingParkHandler;
            validPark = (GameController.Instance.park.guid != null);
            if (validPark)
            {
                initSession();
                GameController.Instance.park.OnNameChanged += onParkNameChangedHandler;
                Parkitect.UI.EventManager.Instance.OnGameSaved += onGameSaved;
            }
            else
            {
                _debug.notification("No valid park found!");
            }
        }

        private void onGameSaved()
        {
            _debug.notification("Game saved");
            addParkFile("save");
            eventsCallCount++;
        }

        private void onParkNameChangedHandler(string oldName, string newName)
        {
            _debug.notification("Park name change to " + newName);
            addParkName(newName);
            eventsCallCount++;
        }

        private void onStartPlayingParkHandler()
        {
            tsSessionStart = getCurrentTimestamp();
            _debug.notification("Started playing at ", tsSessionStart);
            eventsCallCount++;
        }

        private uint getCurrentTimestamp()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            return Convert.ToUInt32(unixTicks.TotalSeconds);
        }

        private void initSession() {
            uint cTs = (tsSessionStart > 0) ? tsSessionStart : getCurrentTimestamp();

            _debug.notification("Init session");
            _debug.notification(_data.loadByHandles());
            if (_data.errorOnLoad) _data = new Data();
            if (_data.tsStart == 0) _data.tsStart = cTs;
            _debug.notification("New data? " + !(_data.sessionIdx > 0));

            _data.tsSessionStarts.Add(cTs);
            _data.sessionIdx++;

            // determine existing park by guid to search for related                 
            if (_data.parks.ContainsKey(GameController.Instance.park.guid))
            {
                _debug.notification("Found park with guid " + GameController.Instance.park.guid);
                _data.currentPark = _data.parks[GameController.Instance.park.guid];
            }
            if (_data.currentPark == null)
            {
                _debug.notification("Add new park with guid " + GameController.Instance.park.guid);
                _data.currentPark = new ParkData();
                _data.currentPark.guid = GameController.Instance.park.guid;
                _data.parks.Add(GameController.Instance.park.guid, _data.currentPark);
            }
            
            if (_data.currentPark.tsStart == 0) _data.currentPark.tsStart = cTs;
            _data.currentPark.tsEnd = cTs;
            _data.currentPark.tsSessionStarts.Add(cTs);
            _data.currentPark.sessionIdx = _data.currentPark.tsSessionStarts.Count - 1;
            _debug.notification("Current session start time ", cTs);

            ParkSessionData _parkDataSession = new ParkSessionData();
            _parkDataSession.tsStart = cTs;
            _parkDataSession.idx = _data.currentPark.sessions.Count;
            _data.currentPark.sessions.Add(_parkDataSession);

            updateData("load");
        }

        private bool addParkFileToData(string name, string mode = "load")
        {
            bool success = false;
            string cName = (_data.currentPark.files.Count > 0) 
                ? _data.currentPark.files[_data.currentPark.files.Count - 1] : null;
            if (cName != name)
            {
                _data.currentPark.files.Add(name);
                if (mode == "save")
                {
                    _data.currentPark.sessions[_data.currentPark.sessionIdx].saveFiles.Add(name);
                } else
                {
                    _data.currentPark.sessions[_data.currentPark.sessionIdx].loadFile = name;
                }
                success = true;
            }
            return success;
        }

        private void addParkFile(string mode = "load")
        {
            ParkData pd = _data.currentPark;
            ParkSessionData pds = pd.sessions[pd.sessionIdx];
            bool updated = false;
            _debug.notification("Add park file");
            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                string[] parkSaveFileElements = GameController.Instance.loadedSavegamePath.Split(
                    (Application.platform == RuntimePlatform.WindowsPlayer) ? '\\' : '/'
                );
                string dFile = parkSaveFileElements[parkSaveFileElements.Length - 1];
                updated = addParkFileToData(dFile, mode);
                if (updated) _debug.notification("New park file " + dFile + " mode (" + mode + ")");
            }
            if (updated == false) _debug.notification("No new park file");
        }

        private bool addParkName(string name)
        {
            ParkData pd = _data.currentPark;
            ParkSessionData pds = pd.sessions[pd.sessionIdx];
            bool success = false;
            if (name != "Unnamed Park" && 
                (pd.names.Count == 0 || pd.names[pd.names.Count - 1] != name))
            {
                _debug.notification("New park name " + name);
                pd.names.Add(name);
                pds.names.Add(name);
                success = true;
            }
            else
            {
                _debug.notification("No new park name");
            }
            return success;
        }

        private void updateData(string mode = "load")
        {
            uint cTs = getCurrentTimestamp();
            _data.tsEnd = cTs;
            _data.currentPark.tsEnd = cTs;
            _debug.notification("Current session end time ", _data.tsEnd);

            ParkSessionData pds = _data.currentPark.sessions[_data.currentPark.sessionIdx];
            _debug.notification("Update park data with session " + pds.idx.ToString());

            _debug.notification("New park time " + ParkInfo.ParkTime.ToString());
            pds.time = Convert.ToUInt32(ParkInfo.ParkTime);
            _data.currentPark.time = pds.time;
            string parkName = GameController.Instance.park.parkName;
            addParkName(parkName);
            addParkFile(mode);
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2 || validPark == false || debugMode == false)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Save Data"))
            {
                updateData("save");
                _debug.notification(_data.saveByHandles());
            }
            if (GUI.Button(new Rect(Screen.width - 200, 30, 200, 20), "Next Session"))
            {
                updateData("save");
                _debug.notification(_data.saveByHandles());
                tsSessionStart = getCurrentTimestamp();
                _data = new Data();
                initSession();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 60, 200, 20), "Reset Data"))
            {
                FilesHandler fh = new FilesHandler();
                fh.deleteAll();
                _debug.notification("All data files have been deleted");
                tsSessionStart = getCurrentTimestamp();
                _data = new Data();
                initSession();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 90, 200, 20), "Debug Data"))
            {
                updateData("save");
                _debug.notification("Current session data");
                _debug.notification("Events proceed " + eventsCallCount.ToString());
                string[] names = { "gameTime", "sessionTime" };
                long[] values = {
                    Convert.ToInt64(_data.tsEnd - _data.tsStart) * 1000,
                    Convert.ToInt64(_data.tsEnd - _data.tsSessionStarts[_data.sessionIdx]) * 1000
                };
                _debug.notification(names, values, 2);

            }
        }

        void OnDisable()
        {
            if (validPark == true)
            {
                GameController.Instance.park.OnNameChanged -= onParkNameChangedHandler;
                Parkitect.UI.EventManager.Instance.OnGameSaved -= onGameSaved;

                updateData("save");
                _data.saveByHandles();
            }
        }

    }
}
