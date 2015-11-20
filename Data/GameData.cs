using System;
using System.Collections.Generic;

namespace StatMaster.Data
{
    class GameData : TimeData
    {
        public string playerGuid = null;

        public bool currentParkOnly = true;
        public ParkData currentPark = null;
        public string currentParkGuid = null;
        public uint currentGameStart = 0;

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
                }
                dict.Add("parkGuids", parkGuids);
            }
            return dict;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);
            if (key == "dataVersion")
            {
                if (dataVersionIdx == 0)
                {
                    // related to dataVersionIdx = 0 in loadByHandles
                    // remove old data and keep main data valid if no newer dataVersionIdx has been loaded
                    // old park guid data will be ignored in the further process automatically
                    fh.deleteAll(); // todo backup method
                }
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
                                nPark.setGuid(parkGuid.ToString());
                                parks.Add(nPark.guid, nPark);

                                if (currentParkGuid == parkGuid.ToString())
                                {
                                    currentPark = nPark;
                                }
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

        public void init()
        {
            if (tsStart == 0) tsStart = Main.getCurrentTimestamp();
            if (playerGuid == null) playerGuid = Guid.NewGuid().ToString();

            tsSessionStarts.Add(currentGameStart);
            sessionIdx = tsSessionStarts.Count - 1;

            if (currentPark == null && currentParkGuid.Length > 0)
            {
                currentPark = new ParkData();
                currentPark.setGuid(currentParkGuid);
            }
            if (!parks.ContainsKey(currentPark.guid))
                parks.Add(currentPark.guid, currentPark);

            currentPark.init();
        }

        public override List<string> loadByHandles()
        {
            uint currentDataVersionIdx = dataVersionIdx;
            dataVersionIdx = 0; // set to 0 to check data version from file inside setObjByKey
            List<string> msgs = base.loadByHandles();
            dataVersionIdx = currentDataVersionIdx;

            List<string> parkGuidsToRemove = new List<string>();
            foreach (string parkGuid in parks.Keys)
            {
                if (currentParkOnly == false || (currentParkGuid == parkGuid))
                {
                    msgs.AddRange(parks[parkGuid].loadByHandles());
                    if (parks[parkGuid].errorOnLoad || parks[parkGuid].invalidDataVersion)
                    {
                        parkGuidsToRemove.Add(parkGuid);
                    }
                    else if (parks.ContainsKey(parkGuid) && currentParkGuid == parkGuid)
                    {
                        currentPark = parks[parkGuid];
                    }
                }
            }
            if (parkGuidsToRemove.Count > 0)
            {
                foreach (string parkGuid in parkGuidsToRemove)
                {
                    parks.Remove(parkGuid);
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
