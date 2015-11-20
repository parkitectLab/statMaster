using MiniJSON;
using System.Collections.Generic;

namespace StatMaster.Data.Park.Progression
{
    class ProgressionPeopleDataValues
    {
        public List<float> peopleMoneyAvg = new List<float>();
        public List<float> peopleHappinessAvg = new List<float>();
        public List<float> peopleTirednessAvg = new List<float>();
        public List<float> peopleHungerAvg = new List<float>();
        public List<float> peopleThirstAvg = new List<float>();
        public List<float> peopleToiletUrgencyAvg = new List<float>();
        public List<float> peopleNauseaAvg = new List<float>();
    }

    class ProgressionPeopleData : ProgressionData
    {

        public ProgressionPeopleData(string parkGuid, int sessionIdx)
        {
            dataVersionIdx = 1;
            minDataVersionIdx = 1;
            setSubFolder("park_" + parkGuid + "/session_" + sessionIdx);
            addHandle("progression_people");
        }

        public override void addRange(Settings settings)
        {
            base.addRange(settings);
            rangeObjects.Add(new ProgressionPeopleDataValues());
        }

        public override void updateRange()
        {
            base.updateRange();

            List<Person> people = GameController.Instance.park.people;
            float moneyAvg = 0f, happinessAvg = 0f, tirednessAvg = 0f, hungerAvg = 0f,
                thirstAvg = 0f, toiletUrgencyAvg = 0f, nauseaAvg = 0f;
            for (int i = 0; i < people.Count; i++)
            {
                moneyAvg += people[i].Money;
                happinessAvg += people[i].Happiness;
                tirednessAvg += people[i].Tiredness;
                hungerAvg += people[i].Hunger;
                thirstAvg += people[i].Thirst;
                toiletUrgencyAvg += people[i].ToiletUrgency;
                nauseaAvg += people[i].Nausea;
            }
            if (moneyAvg > 0f)
                moneyAvg = moneyAvg / people.Count;
            if (happinessAvg > 0f)
                happinessAvg = happinessAvg / people.Count;
            if (tirednessAvg > 0f)
                tirednessAvg = tirednessAvg / people.Count;
            if (hungerAvg > 0f)
                hungerAvg = hungerAvg / people.Count;
            if (thirstAvg > 0f)
                thirstAvg = thirstAvg / people.Count;
            if (toiletUrgencyAvg > 0f)
                toiletUrgencyAvg = toiletUrgencyAvg / people.Count;
            if (nauseaAvg > 0f)
                nauseaAvg = nauseaAvg / people.Count;

            ProgressionPeopleDataValues values = (ProgressionPeopleDataValues)rangeObjects[ranges.Count - 1];
            values.peopleMoneyAvg.Add(moneyAvg);
            values.peopleHappinessAvg.Add(happinessAvg);
            values.peopleTirednessAvg.Add(tirednessAvg);
            values.peopleHungerAvg.Add(hungerAvg);
            values.peopleThirstAvg.Add(thirstAvg);
            values.peopleToiletUrgencyAvg.Add(toiletUrgencyAvg);
            values.peopleNauseaAvg.Add(nauseaAvg);
        }

        protected override Dictionary<string, string> getValuesDict(int idx)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();

            ProgressionPeopleDataValues values = (ProgressionPeopleDataValues)rangeObjects[idx];

            obj.Add("peopleMoneyAvg", Json.Serialize(values.peopleMoneyAvg));
            obj.Add("peopleHappinessAvg", Json.Serialize(values.peopleHappinessAvg));
            obj.Add("peopleTirednessAvg", Json.Serialize(values.peopleTirednessAvg));
            obj.Add("peopleHungerAvg", Json.Serialize(values.peopleHungerAvg));
            obj.Add("peopleThirstAvg", Json.Serialize(values.peopleThirstAvg));
            obj.Add("peopleToiletUrgencyAvg", Json.Serialize(values.peopleToiletUrgencyAvg));
            obj.Add("peopleNauseaAvg", Json.Serialize(values.peopleNauseaAvg));

            return obj;
        }

        protected override bool addValueObj(object obj)
        {
            rangeObjects.Add((ProgressionPeopleDataValues)obj);
            return true;
        }
    }
}
