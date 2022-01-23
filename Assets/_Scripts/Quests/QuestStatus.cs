using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    // �Q�� [System.Serializable]�A�i�����ŧi QuestStatus ���}���A�b Inspector ���X�{���(���ծɨϥΡA��ڤ��ݭn�F�A�]�����ѱ�)
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
