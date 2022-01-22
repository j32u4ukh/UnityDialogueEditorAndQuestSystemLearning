using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public void completeObjective()
        {
            QuestList quest_list = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            quest_list.completeObjective(quest, objective);
        }
    }
}
