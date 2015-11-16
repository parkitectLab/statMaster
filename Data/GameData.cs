using System.Collections.Generic;

namespace StatMaster.Data
{
    class GameData : TimeData
    {

        public bool currentParkOnly = true;
        public ParkData currentPark = null;
        public string currentParkGuid = null;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

        public GameData()
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
                foreach (string parkGuid in parks.Keys)
                {
                    // use parkitect guid + md5 data file name (guid with prefix)
                    parkHandle = fh.calculateMD5Hash("statmaster_data_park_" + parkGuid).ToLower();
                    parksGF.Add(parkGuid, parkHandle);
                    if (currentParkOnly == false || (currentParkGuid == parkGuid))
                    {
                        parks[parkGuid].addHandle("park_" + parkHandle);
                    }
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
                        if (currentParkOnly == false || (currentParkGuid == parkG.ToString())) {
                            ParkData nPark = new ParkData();
                            nPark.guid = parkG.ToString();
                            parkHandle = fh.calculateMD5Hash("statmaster_data_park_" + nPark.guid).ToLower();
                            nPark.addHandle("park_" + parkHandle);
                            parks.Add(parkG.ToString(), nPark);
                        } else
                        {
                            parks.Add(parkG.ToString(), null);
                        }
                    }
                    if (parks.Count == 0) success = false;
                    break;
            }
            return true;
        }

        public override List<string> loadByHandles()
        {
            List<string> msgs = base.loadByHandles();

            foreach (string parkGuid in parks.Keys)
            {
                if (currentParkOnly == false || (currentParkGuid == parkGuid))
                {
                    msgs.AddRange(parks[parkGuid].loadByHandles());
                    errorOnLoad = (parks[parkGuid].errorOnLoad) ? parks[parkGuid].errorOnLoad : errorOnLoad;
                }
            }
            return msgs;
        }

        public override List<string> saveByHandles()
        {
            List<string> msgs = base.saveByHandles();
                        
            foreach (string parkGuid in parks.Keys)
            {
                if (currentParkOnly == false || (currentParkGuid == parkGuid))
                {
                    msgs.AddRange(parks[parkGuid].saveByHandles());
                    errorOnSave = (parks[parkGuid].errorOnSave) ? parks[parkGuid].errorOnSave : errorOnSave;
                }
            }
            return msgs;
        }
    }
}
