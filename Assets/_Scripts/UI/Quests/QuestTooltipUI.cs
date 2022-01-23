using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objective_container;
        [SerializeField] GameObject objective_prefab;
        [SerializeField] GameObject imcompleted_objective_prefab;


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
        }
    }
}