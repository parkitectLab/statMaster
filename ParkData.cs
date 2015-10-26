using System;
using System.Collections.Generic;

namespace StatMaster
{
    [Serializable]
    class ParkData : TimesData
    {
        public List<string> ids = new List<string>();

        public List<ParkSessionData> sessions = new List<ParkSessionData>();

    }
}
