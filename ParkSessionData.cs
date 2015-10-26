using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class ParkSessionData : System.Object
    {
        public int idx;
        public List<string> names = new List<string>(); // to recognize name changes
        public List<string> saveFiles = new List<string>(); // to recognize save file changes

        public uint tsStart = 0; // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint parkTime = 0; // last updated value of ParkInfo.ParkTime in session
    }
}
