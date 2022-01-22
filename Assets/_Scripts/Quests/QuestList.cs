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

        private bool hasQuest(Quest quest)
        {
            foreach(QuestStatus status in statuses)
            {
                if (status.getQuest().Equals(quest))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<QuestStatus> getStatuses()
        {
            return statuses;
        }
    }

}