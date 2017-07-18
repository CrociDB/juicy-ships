using System;
using System.Collections;
using System.Collections.Generic;
using Ship;
using UnityEngine;
using Pool;

namespace Entities
{
    [RequireComponent(typeof(Poolable))]
    [RequireComponent(typeof(Collider))]
    public class Entity : MonoBehaviour
    {
        public float m_ZDistance;

        [SerializeField]
        protected GameObject m_Model;
        protected Poolable m_Poolable;
        protected Collider m_Collider;

        private void Awake()
        {
            m_Poolable = GetComponent<Poolable>();
            m_Collider = GetComponent<Collider>();
        }

        public void Init()
        {
            m_Collider.enabled = true;
            m_Model.SetActive(true);
        }

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
            m_Collider.enabled = false;
            m_Model.SetActive(false);
        }
    }
}
