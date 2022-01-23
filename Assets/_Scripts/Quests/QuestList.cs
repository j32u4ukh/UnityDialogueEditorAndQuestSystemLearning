using GameDevTV.Inventories;
using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
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

            if (status.isComplete())
            {
                giveReward(quest);
            }

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

        private void giveReward(Quest quest)
        {
            foreach(Quest.Reward reward in quest.getRewards())
            {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);

                if (!success)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach(QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            List<object> list = state as List<object>;

            if(list == null)
            {
                return;
            }
            else
            {
                statuses.Clear();

                foreach (object obj in list)
                {
                    statuses.Add(new QuestStatus(obj));
                }
            }
        }
    }

}