using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SectionControl
{
    public class Section : MonoBehaviour
    {
        [HideInInspector]
        public SectionManager m_Manager;

        [SerializeField]
        private float m_DefaultSpace;

        [SerializeField]
        private string[] m_Entities;

        private Vector3 m_Size;

        public Vector3 LastPosition
        {
            get
            {
                return transform.position + m_Size;
            }
        }

        public Vector3 EndPosition
        {
            get
            {
                return transform.position + m_Size + transform.forward * m_DefaultSpace;
            }
        }

        public void Build()
        {
            float farthestDepth = 0f;
            int i = 0;

            foreach (var lane in m_Entities)
            {
                var depth = 0f;
                foreach (var c in lane)
                {
                    depth += BuildEntity(c, depth, i);
                }

                if (depth > farthestDepth)
                {
                    farthestDepth = depth;
                }

                i++;
            }

            m_Size = transform.forward * farthestDepth;
        }

        private float BuildEntity(char c, float depth, int i)
        {
            if (c == 'd')
            {
                return m_DefaultSpace;
            }

            int v;
            if (int.TryParse(c.ToString(), out v))
            {
                var prefab = m_Manager.m_Entities[v];
                var go = Instantiate(prefab, transform);

                go.transform.localPosition = m_Manager.m_Ship.m_Waypoints[0].m_Waypoints[i].position + (go.transform.forward * depth);

                var d = go.GetComponent<Entity>().m_ZDistance;
                return d;
            }

            return 0;
        }

        public void Move(float speed)
        {
            transform.position += transform.forward * -(speed * Time.deltaTime);
        }
    }
}

