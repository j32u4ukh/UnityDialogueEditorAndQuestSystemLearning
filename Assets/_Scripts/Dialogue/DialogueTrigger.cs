using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] string action;
        [SerializeField] UnityEvent onTrigger;

        public void trigger(string action_trigger)
        {
            if (action_trigger.Equals(action))
            {
                onTrigger.Invoke();
            }
        }
    }
}
