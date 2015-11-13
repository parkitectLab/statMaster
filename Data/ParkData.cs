using System;
using System.Collections.Generic;

namespace StatMaster.Data
{
    class ParkData : TimeData
    {
        /*
        public int sessionIdx = 0;
        public List<uint> tsSessionStarts = new List<uint>();

        public uint tsStart = 0; // first timestamp in record to start from
        public uint tsEnd = 0; // last timestamp in record to go to
        */

        public string guid = null;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

        // to recognize all names
        public List<string> names = new List<string>();
        // to recognize all used files
        public List<string> files = new List<string>();

        public bool currentSessionOnly = true;
        public Dictionary<int, ParkSessionData> sessions = new Dictionary<int, ParkSessionData>();

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            if (guid.Length == 0 || sessions.Count == 0) return null;

            dict.Add("guid", guid);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("files", files);

            string sessionHandle = "";
            Dictionary<int, string> sessionsIF = new Dictionary<int, string>();
            foreach (int sIdx in sessions.Keys)
            {
                // use session idx + md5 data file name (idx with prefix)
                sessionHandle = fh.calculateMD5Hash(
                    "statmaster_data_park_" + guid + "_session_" + sIdx
                ).ToLower();
                sessionsIF.Add(sIdx, sessionHandle);
                if (currentSessionOnly == false || (sessionIdx == sIdx))
                {
                    sessions[sIdx].addHandle("park_session_" + sessionHandle);
                }
            }

            dict.Add("sessionsIF", sessionsIF);
            return dict;
        }

        protected override bool setByDictKey(Dictionary<string, object> dict, string key)
        {
            string sessionHandle = "";
            bool success = base.setByDictKey(dict, key);
            switch (key)
            {
                case "guid":
                    guid = dict[key].ToString();
                    if (guid.Length == 0) success = false;
                    break;
                case "time":
                    time = Convert.ToUInt32(dict[key]);
                    break;
                case "names":
                    List<object> dNames = dict[key] as List<object>;
                    if (dNames.Count > 0)
                        foreach (object name in dNames) names.Add(name.ToString());
                    break;
                case "files":
                    List<object> dFiles = dict[key] as List<object>;
                    if (dFiles.Count > 0)
                        foreach (object file in dFiles) files.Add(file.ToString());
                    break;
                case "sessionsIF":
                    Dictionary<string, object> sessionsIF = dict[key] as Dictionary<string, object>;
                    foreach (object sessionI in sessionsIF.Keys)
                    {
                        if (currentSessionOnly == false || (sessionIdx == Convert.ToInt32(sessionI)))
                        {
                            ParkSessionData nSession = new ParkSessionData();
                            nSession.idx = Convert.ToInt32(sessionI);
                            sessionHandle = fh.calculateMD5Hash(
                                "statmaster_data_park_" + guid + "_session_" + nSession.idx
                            ).ToLower();
                            nSession.addHandle("park_session_" + sessionHandle);
                            sessions.Add(nSession.idx, nSession);
                        }
                        else
                        {
                            sessions.Add(Convert.ToInt32(sessionI), null);
                        }
                    }
                    if (sessions.Count == 0) success = false;
                    break;
            }
            return success;
        }

        public override List<string> loadByHandles()
        {
            List<string> msgs = base.loadByHandles();
            if (sessions.Count > 0)
                foreach (int sIdx in sessions.Keys)
                {
                    if (currentSessionOnly == false || (sessionIdx == sIdx))
                    { 
                        msgs.AddRange(sessions[sIdx].loadByHandles());
                        errorOnLoad = (sessions[sIdx].errorOnLoad) ? sessions[sIdx].errorOnLoad : errorOnLoad;
                    }
                }
            return msgs;
        }

        public override List<string> saveByHandles()
        {
            List<string> msgs = base.saveByHandles();
            if (sessions.Count > 0)
                foreach (int sIdx in sessions.Keys)
                {
                    if (currentSessionOnly == false || (sessionIdx == sIdx))
                    {
                        msgs.AddRange(sessions[sIdx].saveByHandles());
                        errorOnSave = (sessions[sIdx].errorOnSave) ? sessions[sIdx].errorOnSave : errorOnSave;
                    }
                }
            return msgs;
        }
    }
}
