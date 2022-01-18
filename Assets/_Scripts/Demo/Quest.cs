using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace udemy
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Ienumerable Primer/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<string> tasks;

        public void addTask(string task)
        {

        }

        public IEnumerable<string> getTasks()
        {
            return tasks;
        }
    }
}
