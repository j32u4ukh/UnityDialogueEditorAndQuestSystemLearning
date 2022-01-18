using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace udemy
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] Quest quest;

        private void Start()
        {
            foreach(string task in quest.getTasks())
            {
                Debug.Log($"Task: {task}");
            }
        }
    }
}
