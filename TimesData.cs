using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class TimesData : System.Object
    {
        // all time related values are unix timestamps
        public int sessionIdx = 0;
        public List<uint> tsSessionStarts = new List<uint>();

        // first timestamp in record to start from
        public uint tsStart = 0;
        // last timestamp in record to go to
        public uint tsEnd = 0;
    }
}
