using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue current_dialogue;

        public string getText()
        {
            if(current_dialogue == null)
            {
                return "";
            }

            return current_dialogue.getRootNode().getText();
        }
    }
}
