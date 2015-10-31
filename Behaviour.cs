using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace StatMaster
{
    class Behaviour : MonoBehaviour
    {
        private Data _data;

        private Debug _debug;

        private bool _deleteDataFileOnDisable = false;

        private int eventsProceed = 0;

        private void Awake()
        {
            
        }

        void Start()
        {
            _debug = new Debug();
            _debug.notification("Start");

            initSession();

            //Parkitect.UI.EventManager.Instance.OnStartPlayingPark += myOnStartPlayingParkHandler;
            GameController.Instance.park.OnNameChanged += myOnParkNameChangedHandler;

            StartCoroutine(autoDataUpdate());
        }

        private void myOnParkNameChangedHandler(string oldName, string newName)
        {
            _debug.notification("Park name change to " + newName);
            addParkName(newName);
            eventsProceed++;
        }

        private void myOnStartPlayingParkHandler()
        {
            eventsProceed++;
        }

        private IEnumerator autoDataUpdate()
        {
            for (;;)
            {
                updateSession();
                yield return new WaitForSeconds(5);
            }
        }

        private uint getCurrentTimestamp()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            return Convert.ToUInt32(unixTicks.TotalSeconds);
        }

        private void initSession() {
            _debug.notification("Init session");

            _data = new Data();
            if (!loadDataFile()) _data = new Data();
            if (_data.tsStart == 0) _data.tsStart = getCurrentTimestamp();
            
            uint cTs = getCurrentTimestamp();
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
            _debug.notificationTs("Current session start time ", cTs);

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

            _debug.notificationTs("Current session end time ", _data.tsEnd);
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Perform Data Actions"))
            {
                updateSession();
                _data.updateHandles();
                List<string> msgs = _data.saveAllHandles();
                foreach (string value in msgs)
                {
                    _debug.notification(value);
                }
                loadDataFile();
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
                _debug.dataNotificationsTimes(names, values, 2);

            }
            if (GUI.Button(new Rect(Screen.width - 200, 60, 200, 20), "Delete Files On Disable"))
            {
                _deleteDataFileOnDisable = (_deleteDataFileOnDisable) ? false : true;
                _debug.notification("Files deletion on disable = " + _deleteDataFileOnDisable.ToString());
            }
        }

        private bool loadDataFile()
        {
            bool success = true;
            _debug.notification("Load data file, not working, needs extensions");
            
            /*if (File.Exists(_data.file))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(_data.file, FileMode.Open);
                    try
                    {
                        try
                        {
                            // todo replace serialization by using MINIJson
                            //_data = (Data)bf.Deserialize(file);
                        }
                        catch (SerializationException e)
                        {
                            _debug.notification("Failed to deserialize. Reason: " + e.Message);
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        _debug.notification("Invalid data on load, reset all data.");
                        success = false;
                    }
                    finally
                    {
                        file.Close();
                    }
                } catch (IOException e)
                {
                    _debug.notification("Failed load. Reason: " + e.Message);
                    success = false;
                }
            }*/

            return success;
        }

        void OnDisable()
        {
            //Parkitect.UI.EventManager.Instance.OnStartPlayingPark -= myOnStartPlayingParkHandler;
            GameController.Instance.park.OnNameChanged -= myOnParkNameChangedHandler;

            if (_deleteDataFileOnDisable)
            {
                //if (File.Exists(_data.file)) File.Delete(_data.file);
            } else
            {
                updateSession();
                _data.updateHandles();
                _data.saveAllHandles();
            }
            
        }

    }
}
