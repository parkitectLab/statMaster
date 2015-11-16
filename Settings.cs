using UnityEngine;
using StatMaster.Data;
using System;

namespace StatMaster
{
    class Settings : MonoBehaviour
    {

        public KeyCode toggleKey = KeyCode.F12;

        public KeyCode activationToggleKey = KeyCode.LeftControl;

        public bool isActive = false;

        public bool devMode = false;

        public bool updateGameData = true;

        public bool updateParkData = true;

        public bool updateParkSessionData = true;

        public bool updateParkProgressionData = true;

        public bool updateFurtherParkProgressionData = true;

        public bool updateAttractionsProgressionData = true;

        public bool updateFurtherAttractionsProgressionData = true;

        public bool updateShopsProgressionData = true;

        public bool updateFurtherShopsProgressionData = true;

        public uint progressionDataUpdateInterval = 10;

        public bool updateAutoSaveData = true;

        public bool ignoreAutoSaveFileNames = true;

        public bool ignoreQuickSaveFileNames = true;

        private uint _windowRectsCount = 14;

        private bool _showWindow = false;

        private Rect _window = new Rect(50, 50, 1, 1);

        private SettingsData _data = new SettingsData();

        void Awake()
        {
            _data.settings = this;
            _data.addHandle("settings");
            _data.loadByHandles();
        }

        void Start()
        {
            // set window to center of screen with default size
            uint windowHeight = _windowRectsCount * 30;
            uint windowWidth = 320;
            _window = new Rect(
                Screen.width / 2 - windowWidth / 2,
                Screen.height / 2 - windowHeight / 2,
                windowWidth,
                windowHeight
           );
        }

        void OnDisable()
        {
            _data.saveByHandles();
        }

        void Update()
        {
            if (Input.GetKeyDown(activationToggleKey))
            {
                isActive = true;
            }
            else if (Input.GetKeyUp(activationToggleKey))
            {
                isActive = false;
            }

            if (isActive == true && (Input.GetKeyDown(toggleKey)))
            {
                _showWindow = !_showWindow;
            }
        }

        private Rect _rect(int index)
        {
            const int height = 20;
            const int margin = 5;

            return new Rect(10, 20 + (height + margin) * index, 300, height);
        }

        private void _doWindow(int id)
        {
            var index = 0;

            updateGameData = GUI.Toggle(_rect(index++), updateGameData, " Update game data");

            if (updateGameData)
            {
                updateParkData = GUI.Toggle(_rect(index++), updateParkData, " Update park data");
            } else
            {
                GUI.Label(_rect(index++), "Update park data (disabled).");
            }

            if (updateGameData && updateParkData)
            {
                updateParkSessionData = GUI.Toggle(_rect(index++), updateParkSessionData, " Update park session data");
            } else
            {
                GUI.Label(_rect(index++), "Update park session data (disabled).");
            }

            if (updateGameData && updateParkData && updateParkSessionData)
            {
                updateParkProgressionData = GUI.Toggle(_rect(index++), updateParkProgressionData, " Update park progression data");
                if (updateParkProgressionData)
                {
                    GUI.Label(_rect(index++), "Update park progression data every " + progressionDataUpdateInterval + " seconds.");
                    progressionDataUpdateInterval = Convert.ToUInt32(
                        GUI.HorizontalSlider(_rect(index++), progressionDataUpdateInterval, 1, 120)
                    ); 
                }
            }
            else
            {
                GUI.Label(_rect(index++), "Update park progression data (disabled).");
            }

            if (updateGameData && updateParkData && updateParkSessionData && updateParkProgressionData)
            {
                updateFurtherParkProgressionData = GUI.Toggle(_rect(index++), updateFurtherParkProgressionData, " Update further park progression data");

                updateAttractionsProgressionData = GUI.Toggle(_rect(index++), updateAttractionsProgressionData, " Update attractions progression data");
                if (updateAttractionsProgressionData)
                {
                    updateFurtherAttractionsProgressionData = GUI.Toggle(_rect(index++), updateFurtherAttractionsProgressionData, " Update further attractions progression data");
                } else
                {
                    GUI.Label(_rect(index++), "Update further attractions progression data (disabled).");
                }

                updateShopsProgressionData = GUI.Toggle(_rect(index++), updateShopsProgressionData, " Update shops progression data");
                if (updateShopsProgressionData)
                {
                    updateFurtherShopsProgressionData = GUI.Toggle(_rect(index++), updateFurtherShopsProgressionData, " Update further shops progression data");
                }
                else
                {
                    GUI.Label(_rect(index++), "Update further shops progression data (disabled).");
                }
            } else
            {
                GUI.Label(_rect(index++), "Update further park progression data (disabled).");
                GUI.Label(_rect(index++), "Update attractions progression data (disabled).");
                GUI.Label(_rect(index++), "Update further attractions progression data (disabled).");
                GUI.Label(_rect(index++), "Update shops progression data (disabled).");
                GUI.Label(_rect(index++), "Update further shops progression data (disabled).");
            }

            if (updateGameData && updateParkData) { 
                updateAutoSaveData = GUI.Toggle(_rect(index++), updateAutoSaveData, " Update AutoSave mod data");
            } else {
                GUI.Label(_rect(index++), "Update AutoSave mod data (disabled).");
            }

            if (updateGameData && updateParkData)
            {
                ignoreAutoSaveFileNames = GUI.Toggle(_rect(index++), ignoreAutoSaveFileNames, " Ignore AutoSave-File names");

                ignoreQuickSaveFileNames = GUI.Toggle(_rect(index++), ignoreQuickSaveFileNames, " Ignore QuickSave-File names");
            } else
            {
                GUI.Label(_rect(index++), "Ignore AutoSave-File names (not active).");
                GUI.Label(_rect(index++), "Ignore QuickSave-File names (not active).");
            }

            devMode = GUI.Toggle(_rect(index++), devMode, " Developer mode with debug messages / actions");

            if (GUI.Button(_rect(index++), "Close")) _showWindow = false;

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        private void OnGUI()
        {
            if (!_showWindow)
                return;

            _window = GUI.Window(0, _window, _doWindow, "StatMaster Settings");
        }
    }
}