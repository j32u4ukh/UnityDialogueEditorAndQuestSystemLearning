using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace udemy
{
    [CreateAssetMenu(fileName = "Unnamed WeaponConfig", menuName = "WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public float max_ammo;
        public float damage;
        public bool area_damage;
    }
}

