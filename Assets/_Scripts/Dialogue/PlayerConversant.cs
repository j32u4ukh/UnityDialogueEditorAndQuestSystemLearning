using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string player_name;
        Dialogue current_dialogue;
        DialogueNode current_node = null;
        AIConversant npc_conversant = null;
        bool is_choosing = false;

        public event Action onConversationUpdated;

        public void startDialogue(AIConversant npc_conversant, Dialogue dialogue)
        {
            this.npc_conversant = npc_conversant;
            current_dialogue = dialogue;
            current_node = current_dialogue.getRootNode();

            triggerEnterAction();
            onConversationUpdated();
        }

        public bool isActive()
        {
            return current_dialogue != null;
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
            triggerExitAction();
            current_node = node;
            triggerEnterAction();
            next();
        }

        public string getCurrentCoversantName()
        {
            if (is_choosing)
            {
                return player_name;
            }
            else
            {
                return npc_conversant.getName();
            }
        }

        public void next()
        {
            int n_response = current_dialogue.getPlayerChildern(root: current_node).Count();

            if(n_response > 0)
            {
                is_choosing = true;
                triggerExitAction();
            }
            else
            {
                is_choosing = false;
                DialogueNode[] children = current_dialogue.getNodeChildren(root: current_node).ToArray();

                triggerExitAction();
                current_node = children[UnityEngine.Random.Range(0, children.Count())];
                triggerEnterAction();
            }

            onConversationUpdated();
        }

        public bool hasNext()
        {
            return current_node.getChildrenNumber() > 0;
        }

        private void triggerEnterAction()
        {
            if(current_node != null)
            {
                Debug.Log(current_node.getEnterAction());
                triggerAction(action: current_node.getEnterAction());
            }
        }

        private void triggerExitAction()
        {
            if (current_node != null)
            {
                Debug.Log(current_node.getExitAction());
                triggerAction(action: current_node.getExitAction());
            }
        }

        void triggerAction(string action)
        {
            if (action.Equals(string.Empty))
            {
                return;
            }

            foreach(DialogueTrigger dt in npc_conversant.GetComponents<DialogueTrigger>())
            {
                dt.trigger(action);
            }
        }

        public void quit()
        {
            triggerExitAction();

            npc_conversant = null;
            current_dialogue = null;
            current_node = null;
            is_choosing = false;

            onConversationUpdated();
        }
    }
}
