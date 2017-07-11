using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Entity : MonoBehaviour
    {
        public float m_ZDistance;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                DestroyMyself();
            }
        }

        protected virtual void DestroyMyself()
        {
            Destroy(gameObject);
        }
    }
}
