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
        public ParkData currentPark = null;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

        public Data()
        {
            addHandle("main");
        }

        public override void updateHandles()
        {
            base.updateHandles();
            foreach (ParkData park in parks.Values)
            {
                park.updateHandles();
            }
        }

        public override List<string> saveAllHandles()
        {
            List<string> msgs = base.saveAllHandles();
            foreach (ParkData park in parks.Values)
            {
                List<string> pMsgs = park.saveAllHandles();
                foreach (string msg in pMsgs) {
                    msgs.Add(msg);
                }
            }
            return msgs;
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = base.getDict(handle);
            string parkHandle = "";
            if (handle == "main")
            {
                dict = base.getDict(handle);

                Dictionary<string, string> parksGF = new Dictionary<string, string>();
                foreach (string key in parks.Keys)
                {
                    // use parkitect guid + md5 data file name (guid with prefix)
                    parkHandle = fh.calculateMD5Hash("statmaster_data_park_" + key).ToLower();
                    parksGF.Add(key, parkHandle);
                    parks[key].addHandle("park_" + parkHandle);
                }
                dict.Add("parksGF", parksGF);
            }
            return dict;
        }
    }
}
