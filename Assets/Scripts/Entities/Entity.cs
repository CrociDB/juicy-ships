using System;
using System.Collections;
using System.Collections.Generic;
using Ship;
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
                TouchedPlayer(other.GetComponent<Ship.ShipObject>());
                DestroyMyself();
            }
        }

        protected virtual void TouchedPlayer(ShipObject shipObject)
        {
        }

        protected virtual void DestroyMyself()
        {
            Destroy(gameObject);
        }
    }
}
