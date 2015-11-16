using System.Collections.Generic;

namespace StatMaster.Data
{
    class GameData : TimeData
    {
        public string playerGuid = null;

        public bool currentParkOnly = true;
        public ParkData currentPark = null;
        public string currentParkGuid = null;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

        public GameData()
        {
            dataVersionIdx = 1;
            addHandle("main");
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = base.getDict(handle);

            if (handle == "main")
            {
                dict = base.getDict(handle);
                dict.Add("playerGuid", playerGuid);

                List<string> parkGuids = new List<string>();
                foreach (string parkGuid in parks.Keys)
                {
                    parkGuids.Add(parkGuid);
                    if (currentParkOnly == false || (currentParkGuid == parkGuid))
                    {
                        parks[parkGuid].addHandle("park_" + parkGuid.ToLower());
                    }
                }
                dict.Add("parkGuids", parkGuids);
            }
            return dict;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);
            if (dataVersionIdx == 0)
            {
                // related to dataVersionIdx = 0 in loadByHandles
                // remove old data and keep main data valid if no newer dataVersionIdx has been loaded
                // old park guid data will be ignored in the further process automatically
                fh.deleteAll();
                dataVersionIdx = 1;
            }

            if (handle == "main")
            {
                switch (key)
                {
                    case "playerGuid":
                        playerGuid = obj.ToString();
                        break;
                    case "parkGuids":
                        List<object> parkGuids = obj as List<object>;
                        foreach (object parkGuid in parkGuids)
                        {
                            if (currentParkOnly == false || (currentParkGuid == parkGuid.ToString()))
                            {
                                ParkData nPark = new ParkData();
                                nPark.guid = parkGuid.ToString();
                                nPark.addHandle("park_" + nPark.guid);
                                parks.Add(nPark.guid, nPark);
                            }
                            else
                            {
                                parks.Add(parkGuid.ToString(), null);
                            }
                        }
                        break;
                }
            }
            return success;
        }

        public override List<string> loadByHandles()
        {
            dataVersionIdx = 0;
            List<string> msgs = base.loadByHandles();

            foreach (string parkGuid in parks.Keys)
            {
                if (currentParkOnly == false || (currentParkGuid == parkGuid))
                {
                    msgs.AddRange(parks[parkGuid].loadByHandles());
                    errorOnLoad = (parks[parkGuid].errorOnLoad) ? parks[parkGuid].errorOnLoad : errorOnLoad;
                    if (!errorOnLoad && parks[parkGuid].invalidDataVersion) parks.Remove(parkGuid);
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
