using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue current_dialogue;
        DialogueNode node = null;

        private void Awake()
        {
            node = current_dialogue.getRootNode();
        }

        public string getText()
        {
            if(node == null)
            {
                return "";
            }

            return node.getText();
        }

        public void next()
        {
            DialogueNode[] children = current_dialogue.getNodeChildren(root: node).ToArray();
            node = children[Random.Range(0, children.Count())];
        }

        public bool hasNext()
        {
            return node.getChildrenNumber() > 0;
        }
    }
}
