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

        private bool debugNotifications = true;

        public void showDebugNotfication(string msg)
        {
            if (debugNotifications)
                Parkitect.UI.NotificationBar.Instance.addNotification("StatMaster: " + msg);
        }

        private void Awake()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }

        private void Start()
        {
            _sw = Stopwatch.StartNew();
            _data = new Data();

            showDebugNotfication("StatMaster Mod Start");
            showDebugNotfication(_data.file);

            loadMainData();

            if (debugNotifications) StartCoroutine(AutoDebugUpdate());
        }

        private IEnumerator AutoDebugUpdate()
        {
            for (;;)
            {
                updateMainData();
                
                yield return new WaitForSeconds(5);
            }
        }

        private void OnGUI()
        {
            if (Application.loadedLevel != 2)
                return;

            if (GUI.Button(new Rect(Screen.width - 200, 0, 200, 20), "Perform Main Data Actions"))
            {
                updateMainData();
                saveMainData();
                loadMainData();
            }
        }

        private void updateMainData()
        {
            long previousCurrentRunTime = _data.currentRunTime;
            _data.currentRunTime = _sw.ElapsedMilliseconds;
            _data.totalRunTime = _data.totalRunTime + _data.currentRunTime - previousCurrentRunTime;
            _data.print(this);
        }

        private void saveMainData()
        {
            showDebugNotfication("Save main data");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(_data.file);
            try
            {
                bf.Serialize(file, _data);
            }
            catch (SerializationException e)
            {
                showDebugNotfication("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                file.Close();
            }
        }

        private void loadMainData()
        {
            showDebugNotfication("Load Main Data");

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
                    showDebugNotfication("Failed to deserialize. Reason: " + e.Message);
                }
                finally
                {
                    file.Close();
                }
            }
            _data.currentRunTime = 0;
        }

        private void OnDisable()
        {
            updateMainData();
            saveMainData();
        }

    }
}
