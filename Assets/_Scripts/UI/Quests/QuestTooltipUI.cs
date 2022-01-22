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


        public void setUp(Quest quest)
        {
            title.text = quest.getTitle();

            foreach(Transform item in objective_container)
            {
                Destroy(item.gameObject);
            }

            foreach(string objective in quest.getObjectives())
            {
                GameObject instance =  Instantiate(objective_prefab, objective_container);
                TextMeshProUGUI objective_text = instance.GetComponentInChildren<TextMeshProUGUI>();
                objective_text.text = objective;
            }
        }
    }
}