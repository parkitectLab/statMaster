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

        public override void updateHandles()
        {
            base.updateHandles();
            foreach (ParkSessionData session in sessions)
            {
                session.updateHandles();
            }
        }

        public override List<string> saveAllHandles()
        {
            List<string> msgs = base.saveAllHandles();
            foreach (ParkSessionData session in sessions)
            {
                List<string> pMsgs = session.saveAllHandles();
                foreach (string msg in pMsgs)
                {
                    msgs.Add(msg);
                }
            }
            return msgs;
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            string sessionHandle = "";
            dict.Add("guid", guid);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("saveFiles", saveFiles);

            Dictionary<int, string> sessionsIF = new Dictionary<int, string>();
            for (int idx = 0; idx < sessions.Count; idx++)
            {
                // use session idx + md5 data file name (idx with prefix)
                sessionHandle = fh.calculateMD5Hash("statmaster_data_park_session_" + idx).ToLower();
                sessionsIF.Add(idx, sessionHandle);
                sessions[idx].addHandle("park_session_" + sessionHandle);
            }
            dict.Add("sessionsIF", sessionsIF);

            return dict;
        }
    }
}
