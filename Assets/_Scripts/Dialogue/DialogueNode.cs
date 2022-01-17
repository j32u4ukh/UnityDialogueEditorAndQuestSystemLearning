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
        public Rect location;
    }
}