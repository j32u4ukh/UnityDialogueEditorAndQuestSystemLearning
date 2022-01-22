using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters;
        [SerializeField] bool activate_on_start = false;

        private void Start()
        {
            activate(should_activate: activate_on_start);
        }

        public void activate(bool should_activate)
        {
            foreach(Fighter fighter in fighters)
            {
                CombatTarget ct = fighter.GetComponent<CombatTarget>();

                if(ct != null)
                {
                    ct.enabled = should_activate;
                }

                fighter.enabled = should_activate;                
            }
        }
    }
}
