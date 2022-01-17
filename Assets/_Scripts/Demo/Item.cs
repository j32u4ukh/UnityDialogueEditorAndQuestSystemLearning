using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace udemy
{
    [CreateAssetMenu(fileName = "Unnamed Item", menuName = "Inventory Item", order = 0)]
    public class Item : ScriptableObject
    {
        [SerializeField] string item_name;
        [SerializeField] string desciption;
        [SerializeField] float weight;

        public string getName()
        {
            return item_name;
        }
    }
}

