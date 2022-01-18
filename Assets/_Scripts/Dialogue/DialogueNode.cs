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
        public string[] children; 
        public Rect rect = new Rect(0f, 0f, 200f, 100f);
    }
}