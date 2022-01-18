using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
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
        /// 編輯器模式下 OnValidate 僅在下面兩種情況下被調用：
        /// 1. 腳本被加載時
        /// 2. Inspector 中的任何值被修改時
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

        public void addChildNode(DialogueNode parent_node)
        {
            DialogueNode child = new DialogueNode();
            parent_node.children.Add(child.unique_id);
            nodes.Add(child);
            node_dict[child.unique_id] = child;
        }

        public void removeNode(DialogueNode node)
        {
            DialogueNode parent = getParentNode(child: node);
            parent.children.Remove(node.unique_id);
            nodes.Remove(node);
            node_dict.Remove(node.unique_id);
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

        public DialogueNode getParentNode(DialogueNode child)
        {
            foreach(DialogueNode node in nodes)
            {
                if (node.children.Contains(child.unique_id))
                {
                    return node;
                }
            }

            return null;
        }
    }
}