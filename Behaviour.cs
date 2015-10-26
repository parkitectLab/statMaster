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
            _debug.notification("Init seession");

            _data = new Data();
            if (!loadDataFile()) _data = new Data();
            if (_data.tsStart == 0) _data.tsStart = getCurrentTimestamp();
            
            uint cTs = getCurrentTimestamp();
            _data.tsSessionStarts.Add(cTs);

            // determine existing save file to search for related 

            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {

            }

                _data.currentPark = new ParkData();
            if (_data.currentPark.tsStart == 0) _data.currentPark.tsStart = getCurrentTimestamp();
            _data.currentPark.tsSessionStarts.Add(cTs);

            ParkSessionData _parkDataSession = new ParkSessionData();
            _parkDataSession.tsStart = cTs;

            updateParkDataSession(_parkDataSession, _data.currentPark);

            _data.currentPark.sessions.Add(_parkDataSession);
        }

        private void updateParkDataSession(ParkSessionData pds, ParkData pd)
        {
            _debug.notification("Update park data session");

            pds.parkTime = Convert.ToUInt32(ParkInfo.ParkTime);
            string parkName = GameController.Instance.park.parkName.ToString();
            if (parkName != "Unamed Park" && (pds.names.Count == 0 || pds.names[pds.names.Count - 1] != parkName))
            {
                pds.names.Add(parkName);
            }

            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                string[] parkSaveFileElements = GameController.Instance.loadedSavegamePath.Split(
                    (Application.platform == RuntimePlatform.WindowsPlayer) ? '\\' : '/'
                );
                string parkSaveFile = parkSaveFileElements[parkSaveFileElements.Length - 1];
                if (parkSaveFileElements.Length > 0 && (pds.saveFiles.Count == 0 || pds.saveFiles[pds.saveFiles.Count - 1] != parkSaveFile))
                {
                    pds.saveFiles.Add(parkSaveFile);
                    string parkId = calculateMD5Hash(parkSaveFile);
                    pd.ids.Add(parkId);
                }
            }
            pds.idx = pd.sessions.Count;
        }

        private void updateSession()
        {
            _debug.notification("Update session");

            _data.tsEnd = getCurrentTimestamp();

            _data.

            
        }

        private IEnumerator autoDataUpdate()
        {
            for (;;)
            {
                updateData();
                yield return new WaitForSeconds(5);
            }
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Perform Data Actions"))
            {
                updateData();
                saveData();
                loadDataFile();
            }
            if (GUI.Button(new Rect(Screen.width - 200, 20, 200, 20), "Debug Current Data"))
            {
                updateData();
                _debug.notification("Current data");
                string[] names = { "gameTime", "gameTimeTotal" };
                long[] values = { _data.gameTime, _data.gameTimeTotal };
                _debug.dataNotificationsTimes(names, values);

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
        

        private void updateParkData(bool start)
        {
            _parkData.time = Convert.ToInt64(ParkInfo.ParkTime) * 1000;
            _parkData.name = GameController.Instance.park.parkName.ToString();
            if (start)
            {
                finalizeParkData();
            }            
        }

        private void finalizeParkData()
        {
            if (File.Exists(GameController.Instance.loadedSavegamePath))
            {
                _parkData.saveGame = GameController.Instance.loadedSavegamePath;
            }
            string lastParkId = _parkData.id;
            if (_parkData.saveGame.Length > 0)
            {
                _parkData.id = calculateMD5Hash(_parkData.saveGame);
                _debug.notification("[HASHED] ParkId " + _parkData.id);
            }
            if (_parkData.id.Length > 0 && lastParkId != _parkData.id)
            {
                if (!_data.parks.ContainsKey(_parkData.id))
                {
                    if (_parkData.rootId != "") _debug.notification("Adding from root park " + _parkData.rootId);
                    if (lastParkId == "")
                    {
                        _parkData.rootId = _parkData.id;
                    }
                    _debug.notification("Add new park " + _parkData.id + " to parks list");
                    _data.parks.Add(_parkData.id, _parkData);
                }
            }
        }

        private void saveData()
        {
            _debug.notification("Save data");

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
                updateData();
                saveData();
            }
            
        }

    }
}
