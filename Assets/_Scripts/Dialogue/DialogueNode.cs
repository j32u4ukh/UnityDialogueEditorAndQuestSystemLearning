using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        public string text;
        public List<string> children; 
        public Rect rect = new Rect(0f, 0f, 200f, 100f);

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
    }
}