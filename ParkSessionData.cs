using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class ParkSessionData : System.Object
    {
        public int idx;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint parkTime = 0;
    }
}
