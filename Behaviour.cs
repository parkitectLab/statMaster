using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System;
using System.Security.Cryptography;
using System.Text;

namespace StatMaster
{
    class Behaviour : MonoBehaviour
    {
        private Stopwatch _sw;

        private Data _data;

        private Debug _debug;

        private ParkData _parkData;

        private bool _deleteDataFileOnDisable = false;

        private void Awake()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }

        void Start()
        {
            _debug = new Debug();
            _debug.notification("Start");

            _sw = Stopwatch.StartNew();

            loadData(false);

            initializePark();

            StartCoroutine(autoDataUpdate());  
        }

        private void initializePark()
        {
            _parkData = new ParkData();
            updateParkData(true);
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
                loadData(true);
            }
            if (GUI.Button(new Rect(Screen.width - 200, 20, 200, 20), "Debug Current Data"))
            {
                updateData();
                _debug.notification("Current data");
                string[] names = { "gameTime", "gameTimeTotal" };
                long[] values = { _data.gameTime, _data.gameTimeTotal };
                _debug.dataNotifications(names, values);

                _debug.notification(_parkData.time.ToString());
                _debug.notification(_parkData.name);
                _debug.notification(_parkData.saveGame);

            }
            if (GUI.Button(new Rect(Screen.width - 200, 40, 200, 20), "Delete Files On Disable"))
            {
                _deleteDataFileOnDisable = (_deleteDataFileOnDisable) ? false : true;
                _debug.notification("Files deletion on disable = " + _deleteDataFileOnDisable.ToString());
            }
        }

        private void updateData()
        {
            _debug.notification("Update data");
            long previousCurrentRunTime = _data.gameTime;
            _data.gameTime = _sw.ElapsedMilliseconds;
            _data.gameTimeTotal = _data.gameTimeTotal + _data.gameTime - previousCurrentRunTime;

            updateParkData(false);
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

        private void updateParkData(bool start)
        {
            _parkData.time = Convert.ToInt64(ParkInfo.ParkTime);
            _parkData.name = GameController.Instance.park.parkName.ToString();
            if (start)
            {
                if (_parkData.gameTimeTotalStart == 0) _parkData.gameTimeTotalStart = _data.gameTimeTotal;
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

        private void loadData(bool reload)
        {
            _debug.notification("Load data");
            _data = new Data();

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
                        _data = new Data();
                    }
                    finally
                    {
                        file.Close();
                    }
                    if (reload == false) _data.gameTime = 0;
                } catch (IOException e)
                {
                    _debug.notification("Failed load. Reason: " + e.Message);
                }
                
            }
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
