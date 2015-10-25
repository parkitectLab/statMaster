
using UnityEngine;
using System;

namespace StatMaster
{
    [Serializable]
    class Data : System.Object
    {

        public string file = Application.persistentDataPath + "/statMaster.dat";

        public long gameTime = 0;
        public long gameTimeTotal = 0;

    }
}
