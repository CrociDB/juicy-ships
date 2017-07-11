using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Ship;
using System;

namespace SectionControl
{
    public class SectionManager : MonoBehaviour
    {
        [Header("Prefab Settings")]
        public Entity[] m_Entities;
        public Section[] m_Sections;

        [Header("Attributes")]
        public float m_Speed = 10f;
        public float m_ThresholdBack = 5f;
        public float m_ThresholdFront = 10f;

        [HideInInspector]
        public ShipController m_Ship;

        private int m_Steps;

        private List<List<Section>> m_CurrentSections;

        public void Init(ShipController shipController)
        {
            m_Ship = shipController;

            m_CurrentSections = new List<List<Section>>();
            foreach (var line in m_Ship.m_Lines)
            {
                var sections = new List<Section>();
                m_CurrentSections.Add(sections);
            }
        }

        void Update()
        {
            var l = 0;
            foreach (var line in m_CurrentSections)
                //var line = m_CurrentSections[0];
            {
                float front = 0f;
                Vector3 endPosition = Vector3.zero;

                foreach (var sec in line)
                {
                    sec.Move(m_Speed);

                    var endPos = sec.EndPosition;

                    var diff = endPos - m_Ship.transform.position;
                    if (diff.sqrMagnitude > front * front && 
                        Vector3.Dot(diff.normalized, m_Ship.transform.forward) > .99f)
                    {
                        front = diff.magnitude;
                        endPosition = endPos;
                    }
                }

                if (front < m_ThresholdFront)
                {
                    CreateSection(l, m_Ship.transform.position + (m_Ship.transform.forward * m_ThresholdFront) + endPosition);
                }

                l++;
            }

            // Remove the ones behind...
            if (++m_Steps > 10)
            {
                m_Steps = 0;
                for (int i = 0; i < m_CurrentSections.Count; i++)
                {
                    var toRemove = m_CurrentSections[i].Where(s =>
                    {
                        var diff = s.EndPosition - m_Ship.transform.position;
                        return (diff.sqrMagnitude > m_ThresholdBack * m_ThresholdBack &&
                            Vector3.Dot(diff.normalized, m_Ship.transform.forward) < -.99f);
                    }).ToArray();

                    for (int j = 0; j < toRemove.Length; j++)
                    {
                        var r = toRemove[j];
                        Destroy(r.gameObject);
                        m_CurrentSections[i].Remove(r);
                    }
                }
            }
        }

        private void CreateSection(int line, Vector3 pos)
        {
            var sectionPrefab = m_Sections[UnityEngine.Random.Range(0, m_Sections.Length)];
            var go = Instantiate(sectionPrefab);
            var section = go.GetComponent<Section>();
            section.m_Manager = this;

            section.transform.position = pos + m_Ship.m_Lines[line].transform.position;
            section.Build();

            m_CurrentSections[line].Add(section);
        }
    }
}

