using MiniJSON;
using System.Collections.Generic;

namespace StatMaster.Data.Park.Progression
{
    class ProgressionFurtherDataValues
    {
        public List<float> money = new List<float>();
        public List<float> entranceFee = new List<float>();
        public List<float> ratingCleanliness = new List<float>();
        public List<float> ratingHappiness = new List<float>();
        public List<float> ratingPriceSatisfaction = new List<float>();
    }

    class ProgressionFurtherData : ProgressionData
    {

        public ProgressionFurtherData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("progression_further");
        }

        public override void addRange()
        {
            base.addRange();
            rangeObjects.Add(new ProgressionFurtherDataValues());
        }

        public void updateRange(
            float money, float entranceFee, float ratingCleanliness, float ratingHappiness,
            float ratingPriceSatisfaction
            )
        {
            base.updateRange();
            ProgressionFurtherDataValues values = (ProgressionFurtherDataValues)rangeObjects[ranges.Count - 1];
            values.money.Add(money);
            values.entranceFee.Add(entranceFee);
            values.ratingCleanliness.Add(ratingCleanliness);
            values.ratingHappiness.Add(ratingHappiness);
            values.ratingPriceSatisfaction.Add(ratingPriceSatisfaction);
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ProgressionFurtherDataValues values = (ProgressionFurtherDataValues)rangeObjects[idx];

            obj.Add("money", Json.Serialize(values.money));
            obj.Add("entranceFee", Json.Serialize(values.entranceFee));
            obj.Add("ratingCleanliness", Json.Serialize(values.ratingCleanliness));
            obj.Add("ratingHappiness", Json.Serialize(values.ratingHappiness));
            obj.Add("ratingPriceSatisfaction", Json.Serialize(values.ratingPriceSatisfaction));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ProgressionFurtherDataValues)obj);
            return true;
        }
    }
}
