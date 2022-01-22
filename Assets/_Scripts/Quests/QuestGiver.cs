using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void giveQuest()
        {
            QuestList quest_list = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            quest_list.addQuest(quest);
        }
    }
}
