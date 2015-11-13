using UnityEngine;
using StatMaster.Data;

namespace StatMaster
{
    class Settings : MonoBehaviour
    {

        public KeyCode toggleKey = KeyCode.F12;

        public KeyCode activationToggleKey = KeyCode.LeftControl;

        public bool isActive = false;

        public bool devMode = false;

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
            devMode = GUI.Toggle(_rect(index++), devMode, "Developer Mode");

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