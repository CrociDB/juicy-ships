using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Ship;
using System;

namespace SectionControl
{
    public class LevelManager : MonoBehaviour
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
        private int m_CurrentSectionCount;

        private Action OnReachEndOfLevel;

        private int m_CurrentLevel;
        private int m_CurrentLevelSections;
        private bool m_Playing;

        public int CurrentLevel
        {
            get
            {
                return m_CurrentLevel;
            }
        }

        public void Init(ShipController shipController, Action reachEndOfLevel)
        {
            m_Ship = shipController;
            OnReachEndOfLevel = reachEndOfLevel;

            m_CurrentSections = new List<List<Section>>();
            foreach (var line in m_Ship.m_Lines)
            {
                var sections = new List<Section>();
                m_CurrentSections.Add(sections);
            }
        }

        internal void SetNextLevel()
        {
            m_CurrentLevel++;
            m_CurrentSectionCount = 0;
            m_CurrentLevelSections = 5 + (m_CurrentLevel / 2);
        }

        internal void PlayGame()
        {
            m_Playing = true;
        }

        internal void StopLevel()
        {
            m_Playing = false;
        }

        void Update()
        {
            if (m_CurrentSections == null || !m_Playing)
            {
                return;
            }

            var l = 0;
            foreach (var line in m_CurrentSections)
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

                // Should we create more sections?
                if (!LevelSectionAchieved())
                {
                    if (line.Count == 0)
                    {
                        CreateSection(l, transform.forward * m_ThresholdFront);
                    }
                    else if (front < m_ThresholdFront)
                    {
                        CreateSection(l, endPosition);
                    }
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
                        r.Destroy();
                        Destroy(r.gameObject);
                        m_CurrentSections[i].Remove(r);
                    }
                }

                if (LevelSectionAchieved() && 
                    m_CurrentSections.Select(sec => sec.Count).Sum() == 0)
                {
                    StopLevel();
                    if (OnReachEndOfLevel != null)
                    {
                        OnReachEndOfLevel.Invoke();
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

            m_CurrentSectionCount++;
            m_CurrentSections[line].Add(section);
        }

        // Level evaluators
        public bool LevelSectionAchieved()
        {
            return m_CurrentSectionCount >= m_CurrentLevelSections * 2;
        }
    }
}

