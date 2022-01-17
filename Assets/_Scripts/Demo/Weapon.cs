using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace udemy
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] WeaponConfig config;

        // Start is called before the first frame update
        void Start()
        {
            shoot();
        }

        public void shoot()
        {
            Debug.Log($"Did {config.damage} damage with {config.name}.");
        }
    }
}

