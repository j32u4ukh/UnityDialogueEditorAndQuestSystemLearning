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

#if UNITY_EDITOR
        public void setText(string text)
        {
            if (!this.text.Equals(text))
            {
                // 紀錄修改歷程，並取代 EditorUtility.SetDirty(selected_dialogue); 將 selected_dialogue 設為 Dirty，
                // 告訴 Unity 這個檔案已被修改，要更新 selected_dialogue 的數據，而不只有更新 Inspector
                // 在實際修改 selected_dialogue 前呼叫，才能回到最初的狀態
                Undo.RecordObject(this, "[DialogueNode] setText");
                this.text = text;
            }
        }

        public void setPosition(Vector2 position)
        {
            Undo.RecordObject(this, "[DialogueNode] setPosition");
            rect.position = position;
        }

        public void addChild(string child_name)
        {
            Undo.RecordObject(this, "[DialogueNode] addChild");
            children.Add(child_name);
        }

        public void removeChild(string child_name)
        {
            Undo.RecordObject(this, "[DialogueNode] removeChild");
            children.Remove(child_name);
        }

#endif
    }
}