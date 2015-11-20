using System;
using System.Collections.Generic;

namespace StatMaster.Data.Park
{
    class ParkSessionData : BaseData
    {
        public int idx = -1;
        // to recognize name changes
        public List<string> names = new List<string>();
        // to recognize save file changes
        public List<string> saveFiles = new List<string>();
        // to recognize a file load
        public string loadFile = "";

        public Progression.ProgressionCountData progressionCountData;
        public Progression.ProgressionFurtherData progressionFurtherData;

        public uint autoSavesCount = 0;
        public uint quickSavesCount = 0;

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;


        public Dictionary<uint, float> attractionsEntranceFeeAvg = new Dictionary<uint, float>();
        public Dictionary<uint, uint> attractionsOpenedCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> attractionsCustomersCount = new Dictionary<uint, uint>();

        public Dictionary<uint, float> shopsProductPriceAvg = new Dictionary<uint, float>();
        public Dictionary<uint, uint> shopsOpenedCount = new Dictionary<uint, uint>();
        public Dictionary<uint, uint> shopsCustomersCount = new Dictionary<uint, uint>();

        public Dictionary<uint, float> peopleMoneyAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleHappinessAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleTirednessAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleHungerAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleThirstAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleToiletUrgencyAvg = new Dictionary<uint, float>();
        public Dictionary<uint, float> peopleNauseaAvg = new Dictionary<uint, float>();

        public ParkSessionData()
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            addHandle("main");
        }

        public void setIdx(string parkGuid, int newIdx)
        {
            idx = newIdx;
            setSubFolder("park_" + parkGuid + "/session_" + newIdx);
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);
            if (idx == -1) return null;

            dict.Add("idx", idx);
            dict.Add("tsStart", tsStart);
            dict.Add("time", time);

            dict.Add("names", names);
            dict.Add("saveFiles", saveFiles);
            dict.Add("loadFile", loadFile);

            dict.Add("autoSavesCount", autoSavesCount);
            dict.Add("quickSaveCount", quickSavesCount);

            dict.Add("attractionsEntranceFeeAvg", attractionsEntranceFeeAvg);
            dict.Add("attractionsOpenedCount", attractionsOpenedCount);
            dict.Add("attractionsCustomersCount", attractionsCustomersCount);

            dict.Add("shopsProductPriceAvg", shopsProductPriceAvg);
            dict.Add("shopsOpenedCount", shopsOpenedCount);
            dict.Add("shopsCustomersCount", shopsCustomersCount);

            dict.Add("peopleMoneyAvg", peopleMoneyAvg);
            dict.Add("peopleHappinessAvg", peopleHappinessAvg);
            dict.Add("peopleTirednessAvg", peopleTirednessAvg);
            dict.Add("peopleHungerAvg", peopleHungerAvg);
            dict.Add("peopleThirstAvg", peopleThirstAvg);
            dict.Add("peopleToiletUrgencyAvg", peopleToiletUrgencyAvg);
            dict.Add("peopleNauseaAvg", peopleNauseaAvg);

            return dict;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);
            switch (key)
            {
                case "idx":
                    idx = Convert.ToInt32(obj);
                    break;
                case "tsStart":
                    tsStart = Convert.ToUInt32(obj);
                    break;
                case "time":
                    time = Convert.ToUInt32(obj);
                    break;
                case "names":
                    List<object> dNames = obj as List<object>;
                    if (dNames.Count > 0)
                        foreach (object name in dNames) names.Add(name.ToString());
                    break;
                case "saveFiles":
                    List<object> dSaveFiles = obj as List<object>;
                    if (dSaveFiles.Count > 0)
                        foreach (object saveFile in dSaveFiles) saveFiles.Add(saveFile.ToString());
                    break;
                case "loadFile":
                    loadFile = (obj.ToString() != null) ? obj.ToString() : "";
                    break;
                case "autoSavesCount":
                    autoSavesCount = Convert.ToUInt32(obj);
                    break;
                case "quickSavesCount":
                    quickSavesCount = Convert.ToUInt32(obj);
                    break;
                case "guestsCount":
                case "employeesCount":
                case "attractionsCount":
                case "shopsCount":
                case "attractionsOpenedCount":
                case "shopsOpenedCount":
                case "attractionsCustomersCount":
                case "shopsCustomersCount":
                    Dictionary<string, object> countValuesDict = obj as Dictionary<string, object>;
                    foreach (string vdKey in countValuesDict.Keys)
                    {
                        uint ts = Convert.ToUInt32(vdKey);
                        uint count = Convert.ToUInt32(countValuesDict[vdKey]);

                        switch (key)
                        {
                            case "attractionsOpenedCount":
                                attractionsOpenedCount.Add(ts, count);
                                break;
                            case "shopsOpenedCount":
                                shopsOpenedCount.Add(ts, count);
                                break;
                            case "attractionsCustomersCount":
                                attractionsCustomersCount.Add(ts, count);
                                break;
                            case "shopsCustomersCount":
                                shopsCustomersCount.Add(ts, count);
                                break;
                        }
                    }
                    break;
                case "money":
                case "ratingCleanliness":
                case "ratingHappiness":
                case "ratingPriceSatisfaction":
                case "entranceFee":
                case "attractionsEntranceFeeAvg":
                case "shopsProductPriceAvg":
                case "peopleMoneyAvg":
                case "peopleHappinessAvg":
                case "peopleTirednessAvg":
                case "peopleHungerAvg":
                case "peopleThirstAvg":
                case "peopleToiletUrgencyAvg":
                case "peopleNauseaAvg":
                    Dictionary<string, object> valuesDict = obj as Dictionary<string, object>;
                    foreach (string vdKey in valuesDict.Keys)
                    {
                        uint ts = Convert.ToUInt32(vdKey);
                        float value = Convert.ToSingle(valuesDict[vdKey]);

                        switch (key)
                        {
                            case "attractionsEntranceFeeAvg":
                                attractionsEntranceFeeAvg.Add(ts, value);
                                break;
                            case "shopsProductPriceAvg":
                                shopsProductPriceAvg.Add(ts, value);
                                break;
                            case "peopleMoneyAvg":
                                peopleMoneyAvg.Add(ts, value);
                                break;
                            case "peopleHappinessAvg":
                                peopleHappinessAvg.Add(ts, value);
                                break;
                            case "peopleTirednessAvg":
                                peopleTirednessAvg.Add(ts, value);
                                break;
                            case "peopleHungerAvg":
                                peopleHungerAvg.Add(ts, value);
                                break;
                            case "peopleThirstAvg":
                                peopleThirstAvg.Add(ts, value);
                                break;
                            case "peopleToiletUrgencyAvg":
                                peopleToiletUrgencyAvg.Add(ts, value);
                                break;
                            case "peopleNauseaAvg":
                                peopleNauseaAvg.Add(ts, value);
                                break;
                        }
                    }
                    break;
            }
            return success;
        }

        public void init(string parkGuid)
        {
            progressionCountData = new Progression.ProgressionCountData(parkGuid, idx);
            progressionCountData.addRange();

            progressionFurtherData = new Progression.ProgressionFurtherData(parkGuid, idx);
            progressionFurtherData.addRange();
        }

        public void update(uint parkTime)
        {
            time = parkTime;

            progressionCountData.updateRange();
            progressionFurtherData.updateRange();
        }

        public override List<string> loadByHandles()
        {
            List<string> msgs = base.loadByHandles();

            msgs.AddRange(progressionCountData.loadByHandles());
            msgs.AddRange(progressionFurtherData.loadByHandles());

            return msgs;
        }

        public override List<string> saveByHandles()
        {
            List<string> msgs = base.saveByHandles();

            msgs.AddRange(progressionCountData.saveByHandles());
            errorOnSave = (progressionCountData.errorOnSave) ? progressionCountData.errorOnSave : errorOnSave;

            msgs.AddRange(progressionFurtherData.saveByHandles());
            errorOnSave = (progressionFurtherData.errorOnSave) ? progressionFurtherData.errorOnSave : errorOnSave;

            return msgs;
        }

    }
}
