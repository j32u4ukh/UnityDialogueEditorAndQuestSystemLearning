using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;
        QuestStatus status;

        public void setUp(QuestStatus status)
        {
            this.status = status;
            Quest quest = status.getQuest();
            title.text = quest.getTitle();
            progress.text = $"{status.getCompletedNumber()}/{quest.getObjectiveNumber()}";
        }

        public QuestStatus getQuestStatus()
        {
            return status;
        }

        public Quest getQuest()
        {
            return status.getQuest();
        }
    }
}

