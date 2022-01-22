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
