using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System;

namespace StatMaster
{
    class Behaviour : MonoBehaviour
    {
        private Stopwatch _sw;

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
            
            _sw = Stopwatch.StartNew();

            loadData(false);

            StartCoroutine(autoDataUpdate());
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
        }

        private void saveData()
        {
            _debug.notification("Save data");

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
        }

        private void loadData(bool reload)
        {
            _debug.notification("Load data");
            _data = new Data();

            if (File.Exists(_data.file))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(_data.file, FileMode.Open);
                try
                {
                    _data = (Data)bf.Deserialize(file);
                }
                catch (SerializationException e)
                {
                    _debug.notification("Failed to deserialize. Reason: " + e.Message);
                }
                finally
                {
                    file.Close();
                }
                if (reload == false) _data.gameTime = 0;
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
