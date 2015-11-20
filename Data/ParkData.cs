using System;
using System.Collections.Generic;

namespace StatMaster.Data
{
    class ParkData : TimeData
    {

        public string guid = null;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

        // to recognize all names
        public List<string> names = new List<string>();
        // to recognize all used files
        public List<string> files = new List<string>();

        public uint autoSavesCount = 0;
        public uint quickSavesCount = 0;

        public bool ignoreSessionsOnFirstLoad = true;
        public bool currentSessionOnly = true;
        public Dictionary<int, ParkSessionData> sessions = new Dictionary<int, ParkSessionData>();

        public ParkData()
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            addHandle("main");
        }

        public void setGuid(string newGuid)
        {
            guid = newGuid;
            setSubFolder("park_" + guid);
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            if (guid.Length == 0 || sessions.Count == 0) return null;

            dict.Add("guid", guid);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("files", files);

            dict.Add("autoSavesCount", autoSavesCount);
            dict.Add("quickSaveCount", quickSavesCount);

            List<int> sessionIdxs = new List<int>();
            foreach (int sIdx in sessions.Keys)
            {
                sessionIdxs.Add(sIdx);
            }

            dict.Add("sessionIdxs", sessionIdxs);
            return dict;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);

            switch (key)
            {
                case "guid":
                    guid = obj.ToString();
                    if (guid.Length == 0) success = false;
                    break;
                case "time":
                    time = Convert.ToUInt32(obj);
                    break;
                case "names":
                    List<object> dNames = obj as List<object>;
                    if (dNames.Count > 0)
                        foreach (object name in dNames) names.Add(name.ToString());
                    break;
                case "files":
                    List<object> dFiles = obj as List<object>;
                    if (dFiles.Count > 0)
                        foreach (object file in dFiles) files.Add(file.ToString());
                    break;
                case "autoSavesCount":
                    autoSavesCount = Convert.ToUInt32(obj);
                    break;
                case "quickSavesCount":
                    quickSavesCount = Convert.ToUInt32(obj);
                    break;
                case "sessionIdxs":
                    List<object> sessionIdxs = obj as List<object>;
                    sessions = new Dictionary<int, ParkSessionData>();
                    foreach (object sIdx in sessionIdxs)
                    {
                        if (ignoreSessionsOnFirstLoad == false &&
                            (currentSessionOnly == false || (sessionIdx == Convert.ToInt32(sIdx))))
                        {
                            ParkSessionData nSession = new ParkSessionData();
                            nSession.setIdx(guid, Convert.ToInt32(sIdx));
                            sessions.Add(nSession.idx, nSession);
                        }
                        else
                        {
                            sessions.Add(Convert.ToInt32(sIdx), null);
                        }
                    }
                    if (sessions.Count == 0) success = false;
                    break;
            }
            return success;
        }

        public void init()
        {
            uint cTs = Main.getCurrentTimestamp();

            if (tsStart == 0)
                tsStart = cTs;
            tsEnd = cTs;

            tsSessionStarts.Add(cTs);
            sessionIdx = tsSessionStarts.Count - 1;

            ParkSessionData parkDataSession = new ParkSessionData();
            parkDataSession.tsStart = tsSessionStarts[tsSessionStarts.Count - 1];
            parkDataSession.setIdx(guid, tsSessionStarts.Count - 1);

            sessions.Add(tsSessionStarts.Count - 1, parkDataSession);
        }

        public override List<string> loadByHandles()
        {
            List<string> msgs = base.loadByHandles();

            List<int> sessionIdxsToRemove = new List<int>();

            if (sessions.Count > 0)
                foreach (int sIdx in sessions.Keys)
                {
                    if (ignoreSessionsOnFirstLoad == false && 
                        (currentSessionOnly == false || (sessionIdx == sIdx)))
                    { 
                        msgs.AddRange(sessions[sIdx].loadByHandles());
                        if (sessions[sIdx].errorOnLoad || sessions[sIdx].invalidDataVersion)
                        {
                            sessionIdxsToRemove.Add(sIdx);
                        } 
                    }
                }

            if (sessionIdxsToRemove.Count > 0)
            {
                foreach (int sIdx in sessionIdxsToRemove)
                {
                    sessions.Remove(sIdx);
                }
            }

            ignoreSessionsOnFirstLoad = false;
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
