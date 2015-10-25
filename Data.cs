using UnityEngine;
using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class Data : System.Object
    {

        public string file = Application.persistentDataPath + "/statMaster.dat";

        public long gameTime = 0;
        public long gameTimeTotal = 0;

        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

    }
}
