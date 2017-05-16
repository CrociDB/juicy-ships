using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Swapping : State
    {
        protected ShipController m_Controller;
        private Transform m_Point;

        public Swapping(Transform point) : base()
        {
            m_Point = point;
        }

        public override void Enter()
        {
            base.Enter();
            m_Controller = (ShipController)m_Entity;

            m_Controller.m_Ship.transform.position = m_Point.position;
            m_Controller.SetState(new Idle());
        }
    }
}
