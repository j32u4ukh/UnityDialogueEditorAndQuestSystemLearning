using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();
        public event Action onListUpdated;

        public void addQuest(Quest quest)
        {
            if (hasQuest(quest))
            {
                return;
            }

            QuestStatus status = new QuestStatus(quest);
            statuses.Add(status);

            if(onListUpdated != null)
            {
                onListUpdated();
            }
        }

        public void completeObjective(Quest quest, string objective)
        {
            QuestStatus status = getQuestStatus(quest);
            status.completeObjective(objective);

            if (onListUpdated != null)
            {
                onListUpdated();
            }
        }

        private bool hasQuest(Quest quest)
        {
            return getQuestStatus(quest) != null;
        }

        private QuestStatus getQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.getQuest().Equals(quest))
                {
                    return status;
                }
            }

            return null;
        }

        public IEnumerable<QuestStatus> getStatuses()
        {
            return statuses;
        }
    }

}