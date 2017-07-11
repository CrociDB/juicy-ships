using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ship
{
    [System.Serializable]
    class ShipLineWaypoints
    {
        public Transform[] m_Waypoints;
    }

    public class ShipController : GameEntity
    {
        [SerializeField]
        internal ShipLineWaypoints[] m_Waypoints;

        internal int m_CurrentWaypointX;
        internal int m_CurrentWaypointY;

        [SerializeField]
        internal Transform[] m_Lines;

        [SerializeField]
        internal GameObject m_Ship;

        public float m_LerpRate = 8f;

        protected override void Start()
        {
            base.Start();
        }

        public void Init()
        {
            m_CurrentWaypointX = 1;
            SetState(new Idle());
        }

        internal Transform GetLeft()
        {
            if (m_CurrentWaypointX == 0)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY].m_Waypoints[m_CurrentWaypointX - 1];
        }

        internal Transform GetRight()
        {
            if (m_CurrentWaypointX == m_Waypoints[m_CurrentWaypointY].m_Waypoints.Length - 1)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY].m_Waypoints[m_CurrentWaypointX + 1];
        }

        internal Transform GetUp()
        {
            if (m_CurrentWaypointY == m_Waypoints.Length - 1)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY + 1].m_Waypoints[m_CurrentWaypointX];
        }

        internal Transform GetDown()
        {
            if (m_CurrentWaypointY == 0)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY - 1].m_Waypoints[m_CurrentWaypointX];
        }

        void Update()
        {

        }
    }
}
