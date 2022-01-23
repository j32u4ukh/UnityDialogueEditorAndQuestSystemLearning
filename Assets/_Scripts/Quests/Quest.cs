using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<string> objectives = new List<string>();

        public string getTitle()
        {
            return name;
        }

        public int getObjectiveNumber()
        {
            return objectives.Count;
        }

        public IEnumerable<string> getObjectives()
        {
            return objectives;
        }

        public bool hasObjective(string objective)
        {
            return objectives.Contains(objective);
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
