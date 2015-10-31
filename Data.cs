using UnityEngine;
using System.Collections.Generic;
using MiniJSON;
using System.Security.Cryptography;
using System.Text;
using System.IO;

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
        private string path = Application.persistentDataPath + "/statMaster/";
        private Dictionary<string, string> files = new Dictionary<string, string>();

        public ParkData currentPark = null;
        public Dictionary<string, ParkData> parks = new Dictionary<string, ParkData>();

        public Data()
        {
            files.Add("main", "main.json");
        }

        public string calculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public string getJson()
        {
            Dictionary<string, object> main = new Dictionary<string, object>();
            main.Add("sessionIdx", sessionIdx);
            main.Add("tsSessionStarts", tsSessionStarts);
            main.Add("tsStart", tsStart);
            main.Add("tsEnd", tsEnd);

            Dictionary<string, string> lParks = new Dictionary<string, string>();
            foreach (string key in parks.Keys)
            {
                // use parkitect guid + md5 data file name (guid with prefix)
                lParks.Add(key, calculateMD5Hash("statmaster_data_park_" + key).ToLower());
            }
            main.Add("parks", lParks);

            return Json.Serialize(main);
        }

        public List<string> save()
        {
            List<string> messages = new List<string>();
            foreach (string key in files.Keys)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    FileStream file = File.Create(path + files[key]);
                    StreamWriter sw = new StreamWriter(file);
                    sw.Write(getJson());
                    sw.Flush();
                    file.Close();
                    messages.Add("Saved file by handle: " + key);
                }
                catch (IOException e)
                {
                    messages.Add("Failed to save file by handle " + key + ". Reason: " + e.Message);
                }
            }
            return messages;

        }
    }
}
