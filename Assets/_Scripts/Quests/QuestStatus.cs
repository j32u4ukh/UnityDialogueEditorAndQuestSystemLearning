using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    // 利用 [System.Serializable]，可讓有宣告 QuestStatus 的腳本，在 Inspector 當中出現欄位
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completed_objectives;

        public Quest getQuest()
        {
            return quest;
        }

        public int getCompletedNumber()
        {
            return completed_objectives.Count;
        }

        public bool isObjectiveComplete(string objective)
        {
            return completed_objectives.Contains(objective);
        }
    }
}
