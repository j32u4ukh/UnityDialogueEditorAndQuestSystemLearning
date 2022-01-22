using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] QuestStatus[] status;

        public IEnumerable<QuestStatus> getStatuses()
        {
            return status;
        }
    }

}