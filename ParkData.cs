using System;
using System.Collections.Generic;

namespace StatMaster
{
    class ParkData : TimesData
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

        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();

        public List<ParkSessionData> sessions = new List<ParkSessionData>();

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            dict.Add("guid", guid);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("saveFiles", saveFiles);

            string sessionHandle = "";
            Dictionary<int, string> sessionsIF = new Dictionary<int, string>();
            foreach (int idx in sessionsIF.Keys)
            {
                // use session idx + md5 data file name (idx with prefix)
                sessionHandle = fh.calculateMD5Hash("statmaster_data_park_session_" + idx).ToLower();
                sessionsIF.Add(idx, sessionHandle);
                sessions[idx].addHandle("park_session_" + sessionHandle);
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
                case "saveFiles":
                    List<object> dSaveFiles = dict[key] as List<object>;
                    if (dSaveFiles.Count > 0)
                        foreach (object saveFile in dSaveFiles) saveFiles.Add(saveFile.ToString());
                    break;
                case "sessionsIF":
                    Dictionary<string, object> sessionsIF = dict[key] as Dictionary<string, object>;
                    foreach (object sessionI in sessionsIF.Keys)
                    {
                        ParkSessionData nSession = new ParkSessionData();
                        nSession.idx = Convert.ToInt32(sessionI);
                        sessionHandle = fh.calculateMD5Hash("statmaster_data_park_session_" + nSession.idx).ToLower();
                        nSession.addHandle("park_session_" + sessionHandle);
                        sessions.Add(nSession);
                    }
                    if (sessions.Count == 0) success = false;
                    break;
            }
            return success;
        }

        public override bool updateHandles(string mode = "set")
        {
            bool success = base.updateHandles(mode);

            if (sessions.Count > 0)
                foreach (ParkSessionData session in sessions)
                {
                    success = success && session.updateHandles(mode);
                }
            return success;
        }

        public override List<string> loadHandles()
        {
            List<string> msgs = base.loadHandles();
            if (sessions.Count > 0)
                foreach (ParkSessionData session in sessions)
                {
                    msgs.AddRange(session.loadHandles());
                }
            return msgs;
        }

        public override List<string> saveHandles()
        {
            List<string> msgs = base.saveHandles();
            if (sessions.Count > 0)
                foreach (ParkSessionData session in sessions)
                {
                    msgs.AddRange(session.saveHandles());
                }
            return msgs;
        }
    }
}
