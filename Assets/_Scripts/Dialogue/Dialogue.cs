using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    // CreateAssetMenu 這行使得在 Asset 當中按右鍵時可產生相對應的檔案
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
                //nodes.Add(DialogueNode.createInstance());
                addChildNode(null);
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
                node_dict[node.name] = node;
            }
        }

        public void addChildNode(DialogueNode parent_node)
        {
            DialogueNode child = DialogueNode.createInstance();
            Undo.RegisterCreatedObjectUndo(child, "Created Dialogue Node");

            if(parent_node != null)
            {
                parent_node.children.Add(child.name);
            }
            
            nodes.Add(child);
            node_dict[child.name] = child;
        }

        public void removeNode(DialogueNode node)
        {
            DialogueNode parent = getParentNode(child: node);
            parent.children.Remove(node.name);
            nodes.Remove(node);
            node_dict.Remove(node.name);
            Undo.DestroyObjectImmediate(node);
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
                if (node.children.Contains(child.name))
                {
                    return node;
                }
            }

            return null;
        }
    }
}