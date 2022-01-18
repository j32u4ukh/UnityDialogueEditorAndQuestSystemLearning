using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string unique_id;
        public string text;
        public List<string> children; 
        public Rect rect;

        public DialogueNode()
        {
            unique_id = System.Guid.NewGuid().ToString();
            text = "";
            children = new List<string>();
            rect = new Rect(0f, 0f, 200f, 100f);
        }
    }
}