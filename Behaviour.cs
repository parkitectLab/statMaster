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

        private bool _deleteDataFileOnDisable = false;

        private uint eventsProceed = 0;

        private uint tsSessionStart = 0;

        private void Awake()
        {
            Parkitect.UI.EventManager.Instance.OnStartPlayingPark += onStartPlayingParkHandler;
        }

        void Start()
        {
            Parkitect.UI.EventManager.Instance.OnStartPlayingPark -= onStartPlayingParkHandler;

            initSession();

            GameController.Instance.park.OnNameChanged += onParkNameChangedHandler;
        }

        private void onParkNameChangedHandler(string oldName, string newName)
        {
            _debug.notification("Park name change to " + newName);
            addParkName(newName);
            eventsProceed++;
        }

        private void onStartPlayingParkHandler()
        {
            tsSessionStart = getCurrentTimestamp();
            _debug.notification("Started playing at ", tsSessionStart);
            eventsProceed++;
        }

        private uint getCurrentTimestamp()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            return Convert.ToUInt32(unixTicks.TotalSeconds);
        }

        private void initSession() {
            Debug.Log("Init session");
            Debug.Log("loaded on init passed, has new data? " + !(_data.sessionIdx > 0));
            if (_data.loadHandles() == null) _data = new Data();
            Debug.Log("loaded on init passed, has new data? " + !(_data.sessionIdx > 0));
            if (_data.tsStart == 0) _data.tsStart = tsSessionStart;

            uint cTs = tsSessionStart;
            _data.tsSessionStarts.Add(cTs);
            _data.sessionIdx++;

            // determine existing park by guid to search for related 
            if (GameController.Instance.park.guid.Length > 0 && 
                _data.parks.ContainsKey(GameController.Instance.park.guid))
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

            updateParkDataSession(_parkDataSession, _data.currentPark);

            _data.currentPark.sessions.Add(_parkDataSession);
        }

        private bool addParkSaveGame(string name)
        {
            bool success = false;
            string cName = (_data.currentPark.saveFiles.Count > 0) 
                ? _data.currentPark.saveFiles[_data.currentPark.saveFiles.Count - 1] : null;
            if (cName != name)
            {
                _data.currentPark.saveFiles.Add(name);
                _data.currentPark.sessions[_data.currentPark.sessionIdx].saveFiles.Add(name);
                success = true;
            }
            return success;
        }

        private void updateParkDataSessionSaveGames(ParkSessionData pds, ParkData pd)
        {
            bool updated = false;
            _debug.notification("Update park data session save games");
            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                string[] parkSaveFileElements = GameController.Instance.loadedSavegamePath.Split(
                    (Application.platform == RuntimePlatform.WindowsPlayer) ? '\\' : '/'
                );
                string dFileName = parkSaveFileElements[parkSaveFileElements.Length - 1];
                updated = addParkSaveGame(dFileName);
                if (updated) _debug.notification("New save game => " + dFileName);
            }
            if (updated == false) _debug.notification("No new save game");
        }

        private bool addParkName(string name)
        {
            bool success = false;
            if (name != "Unnamed Park" && 
                (_data.currentPark.names.Count == 0 || _data.currentPark.names[_data.currentPark.names.Count - 1] != name))
            {
                _debug.notification("New park name " + name);
                _data.currentPark.names.Add(name);
                _data.currentPark.sessions[_data.currentPark.sessionIdx].names.Add(name);
                success = true;
            }
            else
            {
                _debug.notification("No new park name");
            }
            return success;
        }

        private void updateParkDataSession(ParkSessionData pds, ParkData pd)
        {
            _debug.notification("Update park data session");

            _debug.notification("New park time " + ParkInfo.ParkTime.ToString());
            pds.time = Convert.ToUInt32(ParkInfo.ParkTime);
            pd.time = pds.time;
            string parkName = GameController.Instance.park.parkName;

            addParkName(parkName);
            updateParkDataSessionSaveGames(pds, pd);
            
            if (pds.idx == -1) pds.idx = pd.sessions.Count;
            _debug.notification("Current park session index " + pds.idx.ToString());
        }

        private void updateSession()
        {
            _debug.notification("Update session index " + _data.currentPark.sessionIdx.ToString());

            _data.tsEnd = getCurrentTimestamp();
            _data.currentPark.tsEnd = getCurrentTimestamp();

            updateParkDataSession(_data.currentPark.sessions[_data.currentPark.sessionIdx], _data.currentPark);

            _debug.notification("Current session end time ", _data.tsEnd);
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Perform Data Actions"))
            {
                updateSession();
                _debug.notification(_data.saveHandles());
                _debug.notification(_data.loadHandles());
            }
            if (GUI.Button(new Rect(Screen.width - 200, 30, 200, 20), "Debug Current Data"))
            {
                updateSession();
                _debug.notification("Current session data");
                _debug.notification("Events proceed " + eventsProceed.ToString());
                string[] names = { "gameTime", "sessionTime" };
                long[] values = {
                    Convert.ToInt64(_data.tsEnd - _data.tsStart) * 1000,
                    Convert.ToInt64(_data.tsEnd - _data.tsSessionStarts[_data.sessionIdx]) * 1000
                };
                _debug.notification(names, values, 2);

            }
            if (GUI.Button(new Rect(Screen.width - 200, 60, 200, 20), "Delete Files On Disable"))
            {
                _deleteDataFileOnDisable = (_deleteDataFileOnDisable) ? false : true;
                _debug.notification("Files deletion on disable = " + _deleteDataFileOnDisable.ToString());
            }
        }

        void OnDisable()
        {
            GameController.Instance.park.OnNameChanged -= onParkNameChangedHandler;

            if (_deleteDataFileOnDisable)
            {
                FilesHandler fh = new FilesHandler();
                fh.deleteAll();
            } else
            {
                updateSession();
                _data.saveHandles();
            }
            
        }

    }
}
