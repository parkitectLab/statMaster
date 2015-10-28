using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

namespace StatMaster
{
    class Behaviour : MonoBehaviour
    {
        private Data _data;

        private Debug _debug;

        private bool _deleteDataFileOnDisable = false;

        private void Awake()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }

        void Start()
        {

            _debug = new Debug();
            _debug.notification("Start");

            initSession();

            StartCoroutine(autoDataUpdate());
        }

        private uint getCurrentTimestamp()
        {
            TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
            TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
            return Convert.ToUInt32(unixTicks.TotalSeconds);
        }

        private string calculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void initSession() {
            _debug.notification("Init session");

            _data = new Data();
            if (!loadDataFile()) _data = new Data();
            if (_data.tsStart == 0) _data.tsStart = getCurrentTimestamp();
            
            uint cTs = getCurrentTimestamp();
            _data.tsSessionStarts.Add(cTs);

            // determine existing save file to search for related 
            _data.currentPark = new ParkData();
            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                string[] parkIdData = getParkIdData(null);
                if (_data.parks[parkIdData[1]] != null) { 
                  _data.currentPark = _data.parks[parkIdData[1]];
                }
            }

            if (_data.currentPark.tsStart == 0) _data.currentPark.tsStart = getCurrentTimestamp();
            _data.currentPark.tsEnd = getCurrentTimestamp();
            _data.currentPark.tsSessionStarts.Add(cTs);

            ParkSessionData _parkDataSession = new ParkSessionData();
            _parkDataSession.tsStart = cTs;

            updateParkDataSession(_parkDataSession, _data.currentPark);

            _data.currentPark.sessions.Add(_parkDataSession);
        }

        private string[] getParkIdData(ParkSessionData pds)
        {
            _debug.notification("Get park id data");
            string[] data = null;
            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                string[] parkSaveFileElements = GameController.Instance.loadedSavegamePath.Split(
                    (Application.platform == RuntimePlatform.WindowsPlayer) ? '\\' : '/'
                );
                string parkSaveFile = parkSaveFileElements[parkSaveFileElements.Length - 1];
                if (pds == null || (parkSaveFileElements.Length > 0 && (pds.saveFiles.Count == 0 || pds.saveFiles[pds.saveFiles.Count - 1] != parkSaveFile)))
                {
                    data = new string[2];
                    data[0] = parkSaveFile;
                    string parkId = calculateMD5Hash(parkSaveFile);
                    data[1] = parkId;
                }
            }
            return data;
        }

        private void updateParkDataSession(ParkSessionData pds, ParkData pd)
        {
            _debug.notification("Update park data session");

            _debug.notification("New park time " + ParkInfo.ParkTime.ToString());
            pds.parkTime = Convert.ToUInt32(ParkInfo.ParkTime);
            string parkName = GameController.Instance.park.parkName.ToString();
            if (parkName != "Unamed Park" && (pds.names.Count == 0 || pds.names[pds.names.Count - 1] != parkName))
            {
                _debug.notification("New park name " + parkName);
                pds.names.Add(parkName);
            }

            string[] parkIdData = getParkIdData(pds);
            if (parkIdData != null)
            {
                _debug.notification("New save game / park id => " + parkIdData[0] + " / " + parkIdData[1]);
                pds.saveFiles.Add(parkIdData[0]);
                pd.ids.Add(parkIdData[1]);
            }

            pds.idx = pd.sessions.Count;
            _debug.notification("Current session index " + pds.idx);
        }

        private void updateSession()
        {
            _debug.notification("Update session");

            _data.tsEnd = getCurrentTimestamp();
            _data.currentPark.tsEnd = getCurrentTimestamp();

            _debug.notification("Current session end time " + _data.tsEnd);

            updateParkDataSession(_data.currentPark.sessions[_data.currentPark.sessionIdx], _data.currentPark);
        }

        private IEnumerator autoDataUpdate()
        {
            for (;;)
            {
                updateSession();
                yield return new WaitForSeconds(5);
            }
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Perform Data Actions"))
            {
                updateSession();
                saveDataFile();
                loadDataFile();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 20, 200, 20), "Debug Current Data"))
            {
                updateSession();
                _debug.notification("Current session data");
                string[] names = { "gameTime", "sessionTime" };
                long[] values = { _data.tsEnd - _data.tsStart, _data.tsEnd - _data.tsSessionStarts[_data.sessionIdx] };
                _debug.dataNotificationsTimes(names, values, true);

                //_debug.notification(_parkData.time.ToString());
                //_debug.notification(_parkData.name);
                //_debug.notification(_parkData.id);

            }
            if (GUI.Button(new Rect(Screen.width - 200, 40, 200, 20), "Delete Files On Disable"))
            {
                _deleteDataFileOnDisable = (_deleteDataFileOnDisable) ? false : true;
                _debug.notification("Files deletion on disable = " + _deleteDataFileOnDisable.ToString());
            }
        }

        private void saveDataFile()
        {
            _debug.notification("Save data, not working currently");
            return;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Create(_data.file);
                try
                {
                    bf.Serialize(file, _data);
                }
                catch (SerializationException e)
                {
                    _debug.notification("Failed to serialize. Reason: " + e.Message);
                }
                finally
                {
                    file.Close();
                }
            } catch (IOException e)
            {
                _debug.notification("Failed to save. Reason: " + e.Message);
            }
            
        }

        private bool loadDataFile()
        {
            bool invalidData = false;
            _debug.notification("Load data file");
            
            if (File.Exists(_data.file))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file = File.Open(_data.file, FileMode.Open);
                    try
                    {
                        try
                        {
                            _data = (Data)bf.Deserialize(file);
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
                        invalidData = true;
                    }
                    finally
                    {
                        file.Close();
                    }
                } catch (IOException e)
                {
                    _debug.notification("Failed load. Reason: " + e.Message);
                }
            }

            return invalidData;
        }

        void OnDisable()
        {
            if (_deleteDataFileOnDisable)
            {
                if (File.Exists(_data.file)) File.Delete(_data.file);
            } else
            {
                updateSession();
                saveDataFile();
            }
            
        }

    }
}
