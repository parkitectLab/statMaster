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
        public Progression.ProgressionPeopleData progressionPeopleData;
        public Progression.ProgressionAttractionsData progressionAttractionsData;
        public Progression.ProgressionShopsData progressionShopsData;

        public uint autoSavesCount = 0;
        public uint quickSavesCount = 0;

        // start time of session -> equivalent to ParkData.tsSessionStarts[related_idx]
        public uint tsStart = 0;
        // last updated value of ParkInfo.ParkTime in session
        public uint time = 0;

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
            }
            return success;
        }

        public void init(string parkGuid, Settings settings)
        {
            progressionCountData = new Progression.ProgressionCountData(parkGuid, idx);

            progressionFurtherData = new Progression.ProgressionFurtherData(parkGuid, idx);

            progressionPeopleData = new Progression.ProgressionPeopleData(parkGuid, idx);

            progressionAttractionsData = new Progression.ProgressionAttractionsData(parkGuid, idx);

            progressionShopsData = new Progression.ProgressionShopsData(parkGuid, idx);

            initNewProgressionRange(settings);
        }

        public void initNewProgressionRange(Settings settings)
        {

            progressionCountData.updateRangeTime();
            progressionFurtherData.updateRangeTime();
            progressionPeopleData.updateRangeTime();
            progressionAttractionsData.updateRangeTime();
            progressionShopsData.updateRangeTime();

            progressionCountData.addRange(settings);
            progressionFurtherData.addRange(settings);
            progressionPeopleData.addRange(settings);
            progressionAttractionsData.addRange(settings);
            progressionShopsData.addRange(settings);
        }

        public void update(uint parkTime)
        {
            time = parkTime;
        }

        public override List<string> loadByHandles()
        {
            List<string> msgs = base.loadByHandles();

            msgs.AddRange(progressionCountData.loadByHandles());
            msgs.AddRange(progressionFurtherData.loadByHandles());
            msgs.AddRange(progressionPeopleData.loadByHandles());
            msgs.AddRange(progressionAttractionsData.loadByHandles());
            msgs.AddRange(progressionShopsData.loadByHandles());

            return msgs;
        }

        public override List<string> saveByHandles()
        {
            List<string> msgs = base.saveByHandles();

            msgs.AddRange(progressionCountData.saveByHandles());
            errorOnSave = (progressionCountData.errorOnSave) ? progressionCountData.errorOnSave : errorOnSave;

            msgs.AddRange(progressionFurtherData.saveByHandles());
            errorOnSave = (progressionFurtherData.errorOnSave) ? progressionFurtherData.errorOnSave : errorOnSave;

            msgs.AddRange(progressionPeopleData.saveByHandles());
            errorOnSave = (progressionPeopleData.errorOnSave) ? progressionPeopleData.errorOnSave : errorOnSave;

            msgs.AddRange(progressionAttractionsData.saveByHandles());
            errorOnSave = (progressionAttractionsData.errorOnSave) ? progressionAttractionsData.errorOnSave : errorOnSave;

            msgs.AddRange(progressionShopsData.saveByHandles());
            errorOnSave = (progressionShopsData.errorOnSave) ? progressionShopsData.errorOnSave : errorOnSave;

            return msgs;
        }

    }
}
