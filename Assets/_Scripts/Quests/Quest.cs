using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;

        public string getTitle()
        {
            return name;
        }

        public int getObjectiveNumber()
        {
            return objectives.Length;
        }

        public IEnumerable<string> getObjectives()
        {
            return objectives;
        }
    }
}
