using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    // �Q�� [System.Serializable]�A�i�����ŧi QuestStatus ���}���A�b Inspector ���X�{���
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
