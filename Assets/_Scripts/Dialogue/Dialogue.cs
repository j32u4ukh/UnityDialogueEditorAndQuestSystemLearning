using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes;

#if UNITY_EDITOR
        private void Awake()
        {
            if(nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif

        public IEnumerable<DialogueNode> getAllNodes()
        {
            return nodes;
        }

        public DialogueNode getNode(int index = 0)
        {
            index = Math.Min(Math.Max(0, index), nodes.Count - 1);
            return nodes[index];
        }

        public DialogueNode getNode(Vector2 position)
        {
            for(int i = nodes.Count - 1; i >= 0; i--)
            {
                if (nodes[i].rect.Contains(position))
                {
                    return nodes[i];
                }
            }

            return null;
        }

        public DialogueNode getRootNode()
        {
            return nodes[0];
        }
    }
}