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
        Quest quest;

        public void setUp(Quest quest)
        {
            this.quest = quest;
            title.text = quest.getTitle();
            progress.text = $"0/{quest.getObjectiveNumber()}";
        }

        public Quest getQuest()
        {
            return quest;
        }
    }
}

