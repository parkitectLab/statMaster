using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parkitect.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;
using System;

namespace StatMaster
{
    class StatMasterBehaviour : MonoBehaviour
    {
        private Stopwatch _sw;

        private Dictionary<string, string> _mainData;
        private string _mDataFileName;

        private bool debugNotifications = true;

        private void showDebugNotfication(string msg)
        {
            if (debugNotifications)
                Parkitect.UI.NotificationBar.Instance.addNotification("StatMaster: " + msg);
        }

        private void Start()
        {
            _sw = Stopwatch.StartNew();
            _mainData = new Dictionary<string, string>();
            _mDataFileName = Application.persistentDataPath + "/statMaster.dat";

            showDebugNotfication("StatMaster Mod Start");
            showDebugNotfication(_mDataFileName);

            loadMainData();
            initMainData();
            
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

        private void initMainData()
        {
            showDebugNotfication("Init main data");

            string[] keys = { "totalRunTime", "lastRunTime" };

            for (int i = 0; i < keys.Length; i++)
            {
                if (!_mainData.ContainsKey(keys[i]))
                {
                    showDebugNotfication("Add main data key value " + keys[i]);
                    _mainData.Add(keys[i], "0");
                }
            }

            // default values
            _mainData["lastRunTime"] = "0";
        }

        private void updateMainData()
        {
            long lastUpdateElapsedMilliseconds = long.Parse(_mainData["lastRunTime"]);
            long elapsedMilliseconds = _sw.ElapsedMilliseconds;
            long totalElapsedMilliseconds = long.Parse(_mainData["totalRunTime"]) + elapsedMilliseconds - lastUpdateElapsedMilliseconds;

            _mainData["lastRunTime"] = elapsedMilliseconds.ToString();
            _mainData["totalRunTime"] = totalElapsedMilliseconds.ToString();
             
            TimeSpan tsRunTime = TimeSpan.FromMilliseconds(Convert.ToDouble(elapsedMilliseconds));
            TimeSpan tsTotalRunTime = TimeSpan.FromMilliseconds(Convert.ToDouble(totalElapsedMilliseconds));
            
            showDebugNotfication("Main data update " + tsRunTime.ToString() + " / " + tsTotalRunTime.ToString());
        }

        private void saveMainData()
        {
            showDebugNotfication("Save main data");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(_mDataFileName);

            bf.Serialize(file, _mainData);
            file.Close();
        }

        private void loadMainData()
        {
            showDebugNotfication("Load Main Data");

            if (File.Exists(_mDataFileName))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(_mDataFileName, FileMode.Open);
                _mainData = (Dictionary<string, string>)bf.Deserialize(file);
                file.Close();
            }
        }

        private void OnDestroy()
        {
            updateMainData();
            saveMainData();
        }

    }
}
