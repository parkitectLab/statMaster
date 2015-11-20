using MiniJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StatMaster.Data.Park.Progression
{
    class ProgressionAttractionsDataValues
    {
        public List<float> entranceFeeAvg = new List<float>();
        public List<uint> openedCount = new List<uint>();
        public List<uint> customersCount = new List<uint>();
    }

    class ProgressionAttractionsData : ProgressionData
    {

        public ProgressionAttractionsData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("progression_attractions");
        }

        public override void addRange(Settings settings)
        {
            base.addRange(settings);
            rangeObjects.Add(new ProgressionAttractionsDataValues());
        }

        public override void updateRange()
        {
            base.updateRange();
            
            float entranceFeeAvg = 0f;
            ReadOnlyCollection<Attraction> attractions = GameController.Instance.park.getAttractions();
            for (int i = 0; i < attractions.Count; i++)
            {
                entranceFeeAvg += attractions[i].entranceFee;
            }
            if (entranceFeeAvg > 0f)
                entranceFeeAvg = entranceFeeAvg / attractions.Count;

            // todo: improvements to get correct relations 
            // to changes in attractions builded / destroyed status progression
            // related events available? need more info
            uint openedCount = 0;
            uint customersCount = 0;
            for (int i = 0; i < attractions.Count; i++)
            {
                if (attractions[i].state == Attraction.State.OPENED) openedCount++;
                customersCount = Convert.ToUInt32(attractions[i].customersCount);
            }

            ProgressionAttractionsDataValues values = (ProgressionAttractionsDataValues)rangeObjects[ranges.Count - 1];
            values.entranceFeeAvg.Add(entranceFeeAvg);
            values.openedCount.Add(openedCount);
            values.customersCount.Add(customersCount);
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ProgressionAttractionsDataValues values = (ProgressionAttractionsDataValues)rangeObjects[idx];

            obj.Add("entranceFeeAvg", Json.Serialize(values.entranceFeeAvg));
            obj.Add("openedCount", Json.Serialize(values.openedCount));
            obj.Add("customersCount", Json.Serialize(values.customersCount));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ProgressionAttractionsDataValues)obj);
            return true;
        }
    }
}
