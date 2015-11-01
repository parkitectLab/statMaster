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

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = base.getDict(handle);
            string parkHandle = "";
            if (handle == "main")
            {
                dict = base.getDict(handle);
                if (parks.Count == 0) return null;

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
        protected override bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            bool success = base.setByDictKey(dict, key);
            string parkHandle = "";
            switch (key)
            {
                case "parksGF":
                    Dictionary<string, object> parksGF = dict[key] as Dictionary<string, object>;
                    foreach (object parkG in parksGF.Keys)
                    {
                        ParkData nPark = new ParkData();
                        nPark.guid = parkG.ToString();
                        parkHandle = fh.calculateMD5Hash("statmaster_data_park_" + nPark.guid).ToLower();
                        nPark.addHandle("park_" + parkHandle);
                        parks.Add(parkG.ToString(), nPark);
                    }
                    if (parks.Count == 0) success = false;
                    break;
            }
            return true;
        }

        public override bool updateHandles(string mode = "set")
        {
            bool success = base.updateHandles(mode);
            foreach (ParkData park in parks.Values)
            {
                success = success && park.updateHandles(mode);
            }
            return success;
        }

        public override List<string> loadHandles()
        {
            List<string> msgs = base.loadHandles();
            foreach (string parkGuid in parks.Keys)
            {
                msgs.AddRange(parks[parkGuid].loadHandles());
                errorOnLoad = (parks[parkGuid].errorOnLoad) ? parks[parkGuid].errorOnLoad : errorOnLoad;
            }
            return msgs;
        }

        public override List<string> saveHandles()
        {
            List<string> msgs = base.saveHandles();
            
            foreach (string parkGuid in parks.Keys)
            {
                msgs.AddRange(parks[parkGuid].saveHandles());
                errorOnSave = (parks[parkGuid].errorOnSave) ? parks[parkGuid].errorOnSave : errorOnSave;
            }
            return msgs;
        }
    }
}
