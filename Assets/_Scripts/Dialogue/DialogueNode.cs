using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] string text;
        [SerializeField] List<string> children;
        [SerializeField] Rect rect = new Rect(0f, 0f, 200f, 100f);
        [SerializeField] bool is_player_speaking;

        public static DialogueNode createInstance()
        {
            DialogueNode dialogue = CreateInstance<DialogueNode>();
            dialogue.init();
            return dialogue;
        }

        private void init()
        {
            name = System.Guid.NewGuid().ToString();
            text = "";
            children = new List<string>();
            rect = new Rect(0f, 0f, 200f, 100f);
            is_player_speaking = false;
        }

        public string getText()
        {
            return text;
        }

        public Rect getRect()
        {
            return rect;
        }

        public bool containChild(string child_name)
        {
            return children.Contains(child_name);
        }

        public IEnumerable<string> iterChildren()
        {
            return children;
        }

        public int getChildrenNumber()
        {
            return children.Count;
        }

        public bool isPlayerSpeaking()
        {
            return is_player_speaking;
        }

#if UNITY_EDITOR
        public void setText(string text)
        {
            if (!this.text.Equals(text))
            {
                // 紀錄修改歷程，可利用 ctrl + Z 回到上一個狀態
                Undo.RecordObject(this, "[DialogueNode] setText");
                this.text = text;
                EditorUtility.SetDirty(this);
            }
        }

        public void setPosition(Vector2 position)
        {
            Undo.RecordObject(this, "[DialogueNode] setPosition");
            rect.position = position;
            EditorUtility.SetDirty(this);
        }

        public void addChild(string child_name)
        {
            Undo.RecordObject(this, "[DialogueNode] addChild");
            children.Add(child_name);
            EditorUtility.SetDirty(this);
        }

        public void removeChild(string child_name)
        {
            Undo.RecordObject(this, "[DialogueNode] removeChild");
            children.Remove(child_name);
            EditorUtility.SetDirty(this);
        }

        public void setPlayerSpeaking(bool is_player)
        {
            Undo.RecordObject(this, "[DialogueNode] setPlayerSpeaking");
            is_player_speaking = is_player;
            EditorUtility.SetDirty(this);
        }

#endif
    }
}