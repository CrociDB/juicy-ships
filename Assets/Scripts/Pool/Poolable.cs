using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pool
{
    public class Poolable : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnDestroy;
        public UnityEvent OnInit;

        private bool m_Active;
        private Transform m_PoolParent;

        public bool Active
        {
            get
            {
                return m_Active;
            }
        }

        public void InitPool(Transform poolParent)
        {
            m_PoolParent = poolParent;
            m_Active = false;
            gameObject.SetActive(false);
            transform.parent = m_PoolParent;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            m_Active = false;
            if (OnDestroy != null)
            {
                OnDestroy.Invoke();
            }

            transform.parent = m_PoolParent;
            gameObject.transform.localPosition = Vector3.zero;
        }

        public void Activate(Transform parent = null)
        {
            m_Active = true;
            if (OnInit != null)
            {
                OnInit.Invoke();
            }

            gameObject.SetActive(true);
            transform.parent = parent;
        }
    }
}

