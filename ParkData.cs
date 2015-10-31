using System.Collections.Generic;

namespace StatMaster
{
    class ParkData : TimesData
    {
        /*
        public int sessionIdx = 0;
        public List<uint> tsSessionStarts = new List<uint>();

        public uint tsStart = 0; // first timestamp in record to start from
        public uint tsEnd = 0; // last timestamp in record to go to
        */

        public string guid = null;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();

        // last updated value of ParkInfo.ParkTime in session
        public uint parkTime = 0; 

        public List<ParkSessionData> sessions = new List<ParkSessionData>();

    }
}
