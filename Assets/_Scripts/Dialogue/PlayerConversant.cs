using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue current_dialogue;
        DialogueNode current_node = null;
        bool is_choosing = false;

        private void Awake()
        {
            current_node = current_dialogue.getRootNode();
        }

        public bool isChoosing()
        {
            return is_choosing;
        }

        public string getText()
        {
            if(current_node == null)
            {
                return "";
            }

            return current_node.getText();
        }

        public IEnumerable<DialogueNode> getChoices()
        {
            return current_dialogue.getPlayerChildern(root: current_node);
        }

        public void selectChoice(DialogueNode node)
        {
            current_node = node;
            next();
        }

        public void next()
        {
            int n_response = current_dialogue.getPlayerChildern(root: current_node).Count();

            if(n_response > 0)
            {
                is_choosing = true;
                return;
            }

            is_choosing = false;
            DialogueNode[] children = current_dialogue.getNodeChildren(root: current_node).ToArray();
            current_node = children[Random.Range(0, children.Count())];
        }

        public bool hasNext()
        {
            return current_node.getChildrenNumber() > 0;
        }
    }
}
