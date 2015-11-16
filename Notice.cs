using UnityEngine;
using System.Collections.Generic;
using System;

namespace StatMaster
{
    class Notice
    {

        public bool showWindow = false;

        public uint windowWidth = 450;

        private Rect _window = new Rect(50, 50, 1, 1);

        private List<string> _textLines = new List<string>();

        public string title = "StatMaster Notice";

        private bool _updateWindowRect = false;

        public void addText(string text)
        {
            _textLines.Add(text);
            _updateWindowRect = true;
        }

        public void addText(List<string>texts)
        {
            _textLines.AddRange(texts);
            _updateWindowRect = true;
        }

        private Rect _rect(int index)
        {
            const int height = 20;
            const int margin = 5;

            return new Rect(10, 20 + (height + margin) * index, windowWidth - 20, height);
        }

        private void _doWindow(int id)
        {
            var index = 0;

            for (int i = 0; i < _textLines.Count; i++)
                GUI.Label(_rect(index++), _textLines[i]);

            if (GUI.Button(_rect(index++), "Close")) showWindow = false;

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }

        public void OnGUI()
        {
            if (!showWindow)
                return;

            if (_updateWindowRect)
            {
                // set window to center of screen with default size
                uint windowHeight = Convert.ToUInt32(_textLines.Count) * 30 + 30;
                _window = new Rect(
                    Screen.width / 2 - windowWidth / 2,
                    Screen.height / 2 - windowHeight / 2,
                    windowWidth,
                    windowHeight
               );

               _updateWindowRect = false;
            }

            _window = GUI.Window(0, _window, _doWindow, title);
        }
    }
}