using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    // CreateAssetMenu 這行使得在 Asset 當中按右鍵時可產生相對應的檔案
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 new_node_offset = new Vector2(250f, 0f);
        Dictionary<string, DialogueNode> node_dict = new Dictionary<string, DialogueNode>();

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

        // When save a file to hard drive
        // Implement this method to receive a callback before Unity serializes your object.
        public void OnBeforeSerialize()
        {
            // 宏寫在此處而非函式外面，是因為繼承了 ISerializationCallbackReceiver 就必須實作此函式，不能只在編輯器模式下有這個函式
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                //nodes.Add(DialogueNode.createInstance());
                addNode(null, use_undo: false);
            }

            if (!AssetDatabase.GetAssetPath(this).Equals(string.Empty))
            {
                foreach(DialogueNode node in nodes)
                {
                    if (AssetDatabase.GetAssetPath(node).Equals(string.Empty))
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        // When load a file from hard drive
        // Implement this method to receive a callback after Unity deserializes your object.
        public void OnAfterDeserialize()
        {
            
        }

#if UNITY_EDITOR
        public void addNode(DialogueNode root, bool use_undo = true)
        {
            DialogueNode node = DialogueNode.createInstance();
            //Debug.Log($"addNode: {node.name}");

            if (root != null)
            {
                root.addChild(node.name);
                node.setPlayerSpeaking(!root.isPlayerSpeaking());
                node.setPosition(root.getRect().position + new_node_offset);
            }

            if (use_undo)
            {
                Undo.RegisterCreatedObjectUndo(node, "[Dialogue] addNode | Created Dialogue Node");
                Undo.RecordObject(this, "[Dialogue] addNode | Add New Dialogue Node.");
            }
            
            nodes.Add(node);
            //node_dict[node.name] = node;
            OnValidate();
        }

        public void removeNode(DialogueNode node)
        {
            Undo.RecordObject(this, "[Dialogue] removeNode");
            DialogueNode parent = getParentNode(child: node);

            if(parent != null)
            {
                parent.removeChild(node.name);
            }
            
            nodes.Remove(node);
            //node_dict.Remove(node.name);
            OnValidate();
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
                if (nodes[i].getRect().Contains(position))
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
            foreach (string unique_id in root.iterChildren())
            {
                if (node_dict.ContainsKey(unique_id))
                {
                    yield return node_dict[unique_id];
                }
            }
        }

        public IEnumerable<DialogueNode> getPlayerChildern(DialogueNode root)
        {
            foreach(DialogueNode node in getNodeChildren(root))
            {
                if (node.isPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> getComChildern(DialogueNode root)
        {
            foreach(DialogueNode node in getNodeChildren(root))
            {
                if (!node.isPlayerSpeaking())
                {
                    yield return node;
                }
            }
        }

        public DialogueNode getParentNode(DialogueNode child)
        {
            foreach(DialogueNode node in nodes)
            {
                if (node.containChild(child.name))
                {
                    return node;
                }
            }

            return null;
        }
#endif

    }
}