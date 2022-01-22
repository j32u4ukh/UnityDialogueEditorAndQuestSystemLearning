using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    // 利用 [System.Serializable]，可讓有宣告 QuestStatus 的腳本，在 Inspector 當中出現欄位(測試時使用，實際不需要了，因此註解掉)
    //[System.Serializable]
    public class QuestStatus
    {
        Quest quest;
        List<string> completed_objectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

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
