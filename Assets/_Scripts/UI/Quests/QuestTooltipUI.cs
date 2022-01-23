using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;
using System;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objective_container;
        [SerializeField] GameObject objective_prefab;
        [SerializeField] GameObject imcompleted_objective_prefab;
        [SerializeField] TextMeshProUGUI reward;

        public void setUp(QuestStatus status)
        {
            Quest quest = status.getQuest();
            title.text = quest.getTitle();

            foreach(Transform item in objective_container)
            {
                Destroy(item.gameObject);
            }

            foreach(Quest.Objective objective in quest.getObjectives())
            {
                GameObject instance;

                if (status.isObjectiveComplete(objective.reference))
                {
                    instance = Instantiate(objective_prefab, objective_container);
                }
                else
                {
                    instance = Instantiate(imcompleted_objective_prefab, objective_container);
                }

                TextMeshProUGUI objective_text = instance.GetComponentInChildren<TextMeshProUGUI>();
                objective_text.text = objective.description;
            }

            reward.text = getRewardText(quest);
        }

        private string getRewardText(Quest quest)
        {
            string text = "";

            foreach (Quest.Reward reward in quest.getRewards())
            {
                if (!text.Equals(string.Empty))
                {
                    text += ", ";
                }

                if(reward.number > 1)
                {
                    text += $"{reward.number} ";
                }

                text += reward.item.GetDisplayName();
            }

            if (text.Equals(string.Empty))
            {
                text = "No reward";
            }

            text += ".";

            return text;
        }
    }
}