using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        public string getTitle()
        {
            return name;
        }

        public int getObjectiveNumber()
        {
            return objectives.Count;
        }

        public IEnumerable<Reward> getRewards()
        {
            return rewards;
        }

        public IEnumerable<Objective> getObjectives()
        {
            return objectives;
        }

        public bool hasObjective(string reference)
        {
            foreach (Objective objective in objectives)
            {
                if (reference.Equals(objective.reference))
                {
                    return true;
                }
            }

            return false;
        }

        public static Quest getByName(string quest_name)
        {
            foreach(Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name.Equals(quest_name))
                {
                    return quest;
                }
            }

            return null;
        }
    }
}
