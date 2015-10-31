using UnityEngine;
using System.Collections.Generic;

namespace StatMaster
{
    class Data : TimesData
    {
        /*
        public int sessionIdx = 0;
        public List<uint> tsSessionStarts = new List<uint>();

        public uint tsStart = 0; // first timestamp in record to start from
        public uint tsEnd = 0; // last timestamp in record to go to
        */
        public string file = Application.persistentDataPath + "/statMaster.dat";

        public ParkData currentPark = null;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

    }
}
