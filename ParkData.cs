using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class ParkData : TimesData
    {
        /*
        public int sessionIdx = 0;
        public List<uint> tsSessionStarts = new List<uint>();

        public uint tsStart = 0; // first timestamp in record to start from
        public uint tsEnd = 0; // last timestamp in record to go to
        */
        public List<string> ids = new List<string>();


        public int idx;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint parkTime = 0; 

        public List<ParkSessionData> sessions = new List<ParkSessionData>();

    }
}
