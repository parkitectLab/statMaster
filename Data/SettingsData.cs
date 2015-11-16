using System.Collections.Generic;
using System;

namespace StatMaster.Data
{
    class SettingsData : BaseData
    {

        public Settings settings;

        public SettingsData()
        {
            dataVersionIdx = 1;
        }

        protected override Dictionary<string, object> getDict(string handle)
        {
            Dictionary<string, object> dict = base.getDict(handle);

            dict.Add("devMode", settings.devMode);

            dict.Add("updateGameData", settings.updateGameData);
            dict.Add("updateParkData", settings.updateParkData);
            dict.Add("updateParkSessionData", settings.updateParkSessionData);

            dict.Add("updateProgressionData", settings.updateProgressionData);
            dict.Add("progressionDataUpdateInterval", settings.progressionDataUpdateInterval);

            dict.Add("updatePeopleData", settings.updatePeopleData);
            dict.Add("updateAttractionsData", settings.updateAttractionsData);
            dict.Add("updateShopsnData", settings.updateShopsData);

            dict.Add("updateAutoSaveData", settings.updateAutoSaveData);
            dict.Add("ignoreQuickSaveFileNames", settings.ignoreQuickSaveFileNames);
            dict.Add("ignoreAutoSaveFileNames", settings.ignoreAutoSaveFileNames);

            return dict;
        }

        protected override bool setObjByKey(string handle, string key, object obj)
        {
            bool success = base.setObjByKey(handle, key, obj);

            switch (key)
            {
                case "devMode":
                    settings.devMode = Convert.ToBoolean(obj);
                    break;
                case "updateGameData":
                    settings.updateGameData = Convert.ToBoolean(obj);
                    break;
                case "updateParkData":
                    settings.updateParkData = Convert.ToBoolean(obj);
                    break;
                case "updateParkSessionData":
                    settings.updateParkSessionData = Convert.ToBoolean(obj);
                    break;
                case "updateProgressionData":
                    settings.updateProgressionData = Convert.ToBoolean(obj);
                    break;
                case "progressionDataUpdateInterval":
                    settings.progressionDataUpdateInterval = Convert.ToUInt32(obj);
                    break;
                case "updatePeopleData":
                    settings.updatePeopleData = Convert.ToBoolean(obj);
                    break;
                case "updateAttractionsData":
                    settings.updateAttractionsData = Convert.ToBoolean(obj);
                    break;
                case "updateShopsData":
                    settings.updateShopsData = Convert.ToBoolean(obj);
                    break;                
                case "updateAutoSaveData":
                    settings.updateAutoSaveData = Convert.ToBoolean(obj);
                    break;
                case "ignoreQuickSaveFileNames":
                    settings.ignoreQuickSaveFileNames = Convert.ToBoolean(obj);
                    break;
                case "ignoreAutoSaveFileNames":
                    settings.ignoreAutoSaveFileNames = Convert.ToBoolean(obj);
                    break;
            }

            return true;
        }
    }
}
