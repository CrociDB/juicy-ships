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
        private Transform m_CurrentWaypoint;

        public override void Enter()
        {
            base.Enter();
            m_Controller = (ShipController)m_Entity;
            m_CurrentWaypoint = m_Controller.m_Waypoints[m_Controller.m_CurrentWaypointY].m_Waypoints[m_Controller.m_CurrentWaypointX];
        }

        public override void Update()
        {
            base.Update();

            m_Controller.m_Ship.transform.position = Vector3.Lerp(
                m_Controller.m_Ship.transform.position,
                m_CurrentWaypoint.position,
                Time.deltaTime * m_Controller.m_LerpRate);

            m_Controller.m_Ship.transform.rotation = Quaternion.Lerp(
                m_Controller.m_Ship.transform.rotation,
                Quaternion.identity,
                Time.deltaTime * m_Controller.m_LerpRate * .7f);

            if (Input.GetKeyDown(KeyCode.A))
            {
                GoLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                GoRight();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GoUp();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GoDown();
            }
        }

        private void GoLeft()
        {
            var waypoint = m_Controller.GetLeft();
            if (waypoint != null)
            {
                m_Controller.m_CurrentWaypointX--;
                m_Controller.SetState(new Swapping(waypoint, false));
            }
        }

        private void GoRight()
        {
            var waypoint = m_Controller.GetRight();
            if (waypoint != null)
            {
                m_Controller.m_CurrentWaypointX++;
                m_Controller.SetState(new Swapping(waypoint, false));
            }
        }

      private void GoUp()
      {
         var waypoint = m_Controller.GetUp();
         if (waypoint != null)
         {
            m_Controller.m_CurrentWaypointY++;
            m_Controller.SetState(new Swapping(waypoint, true));
         }
      }

      private void GoDown()
      {
         var waypoint = m_Controller.GetDown();
         if (waypoint != null)
         {
            m_Controller.m_CurrentWaypointY--;
            m_Controller.SetState(new Swapping(waypoint, true));
         }
      }
   }
}
