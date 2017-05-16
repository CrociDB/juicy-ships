using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class ShipController : GameEntity
    {
        [SerializeField]
        private Transform[] m_Waypoints;
        internal int m_CurrentWaypoint;

        [SerializeField]
        internal GameObject m_Ship;

        protected override void Start()
        {
            base.Start();
            m_CurrentWaypoint = 1;

            SetState(new Idle());
        }

        internal Transform GetLeft()
        {
            if (m_CurrentWaypoint == 0)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypoint - 1];
        }

        internal Transform GetRight()
        {
            if (m_CurrentWaypoint == m_Waypoints.Length - 1)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypoint + 1];
        }

        void Update ()
        {
		
	    }
    }
}
