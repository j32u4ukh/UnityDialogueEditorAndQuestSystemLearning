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
        Dictionary<string, DialogueNode> node_dict = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif
        /// <summary>
        /// �s�边�Ҧ��U OnValidate �Ȧb�U����ر��p�U�Q�եΡG
        /// 1. �}���Q�[����
        /// 2. Inspector ��������ȳQ�ק��
        /// </summary>
        private void OnValidate()
        {
            node_dict.Clear();

            foreach (DialogueNode node in nodes)
            {
                //node_dict.Add(node.unique_id, node);
                node_dict[node.unique_id] = node;
            }
        }

        public IEnumerable<DialogueNode> getAllNodes()
        {
            return nodes;
        }

        public DialogueNode getNode(int index = 0)
        {
            if ((index < 0) || (nodes.Count <= index))
            {
                return null;
            }

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

        public DialogueNode getNode(string unique_id)
        {
            if (node_dict.ContainsKey(unique_id))
            {
                return node_dict[unique_id];
            }

            return null;
        }

        public DialogueNode getRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> getNodeChildren(DialogueNode root)
        {
            foreach (string unique_id in root.children)
            {
                if (node_dict.ContainsKey(unique_id))
                {
                    yield return node_dict[unique_id];
                }
            }
        }
    }
}