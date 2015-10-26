using UnityEngine;
using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class Data : TimesData
    {

        public string file = Application.persistentDataPath + "/statMaster.dat";

        public ParkData currentPark;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

    }
}
