using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] quests;
        [SerializeField] QuestItemUI quest_prefab;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            QuestList quest_list = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            foreach (QuestStatus status in quest_list.getStatuses())
            {
                QuestItemUI ui_instance = Instantiate(quest_prefab, transform);
                ui_instance.setUp(status);
            }
        }
    }
}

