using System;
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

        [System.Serializable]
        class QuestStatusRecord
        {
            public string quest_name;
            public List<string> completed_objectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object obj)
        {
            QuestStatusRecord record = obj as QuestStatusRecord;
            quest = Quest.getByName(quest_name: record.quest_name);
            completed_objectives = record.completed_objectives;
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

        public void completeObjective(string objective)
        {
            if (quest.hasObjective(objective))
            {
                completed_objectives.Add(objective);
            }
        }

        public object CaptureState()
        {
            QuestStatusRecord record = new QuestStatusRecord();
            record.quest_name = quest.name;
            record.completed_objectives = completed_objectives;

            return record;
        }
    }
}
