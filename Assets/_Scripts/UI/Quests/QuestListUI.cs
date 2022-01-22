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
        QuestList quest_list;

        // Start is called before the first frame update
        void Start()
        {
            quest_list = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            quest_list.onListUpdated += updateQuestListUI;

            updateQuestListUI();
        }

        void updateQuestListUI()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            foreach (QuestStatus status in quest_list.getStatuses())
            {
                QuestItemUI ui_instance = Instantiate(quest_prefab, transform);
                ui_instance.setUp(status);
            }
        }
    }
}

