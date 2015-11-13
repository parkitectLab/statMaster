﻿using UnityEngine;
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

        public bool updateProgressionData = true;

        public uint dataUpdateInterval = 10;

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
            const int windowHeight = 140;
            const int windowWidth = 320;
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

            return new Rect(5, 20 + (height + margin) * index, 300, height);
        }

        private void _doWindow(int id)
        {
            var index = 0;

            updateGameData = GUI.Toggle(_rect(index++), updateProgressionData, " Update game data");

            updateParkData = GUI.Toggle(_rect(index++), updateProgressionData, " Update park data");

            updateProgressionData = GUI.Toggle(_rect(index++), updateProgressionData, " Update park progression data");

            GUI.Label(_rect(index++), "Update park progression data every " + dataUpdateInterval + " seconds.");
            dataUpdateInterval = Convert.ToUInt32(
                GUI.HorizontalSlider(_rect(index++), dataUpdateInterval, 1, 120)
            );

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