using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatMaster
{
    [Serializable]
    class ParkData : System.Object
    {
        public string rootId = "";
        public string id = "";
        public string name = "";
        public string saveGame = "";
        public long time = 0;
        public long gameTimeTotalStart = 0;
        public long gameTime = 0;

    }
}
