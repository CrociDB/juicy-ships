using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ship
{
    public class Idle : State
    {
        protected ShipController m_Controller;

        public override void Enter()
        {
            base.Enter();
            m_Controller = (ShipController)m_Entity;
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.A))
            {
                GoLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                GoRight();
            }
        }

        private void GoLeft()
        {
            var waypoint = m_Controller.GetLeft();
            if (waypoint != null)
            {
                m_Controller.m_CurrentWaypoint--;
                m_Controller.SetState(new Swapping(waypoint));
            }
        }

        private void GoRight()
        {
            var waypoint = m_Controller.GetRight();
            if (waypoint != null)
            {
                m_Controller.m_CurrentWaypoint++;
                m_Controller.SetState(new Swapping(waypoint));
            }
        }
    }
}
