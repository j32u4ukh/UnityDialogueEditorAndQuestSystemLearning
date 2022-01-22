using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] temp_quests;
        [SerializeField] QuestItemUI quest_prefab;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            foreach (Quest quest in temp_quests)
            {
                QuestItemUI ui_instance = Instantiate(quest_prefab, transform);
                ui_instance.setUp(quest);
            }
        }
    }
}

