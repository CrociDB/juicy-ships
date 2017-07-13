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
        private bool m_Vertical;

        public Swapping(Transform point, bool vertical) : base()
        {
            m_Point = point;
            m_Vertical = vertical;
        }

        public override void Enter()
        {
            base.Enter();
            m_Controller = (ShipController)m_Entity;
        }

        public override void Update()
        {
            base.Update();
            m_Controller.m_Ship.transform.position = Vector3.Lerp(
                m_Controller.m_Ship.transform.position,
                m_Point.position,
                Time.deltaTime * m_Controller.m_LerpRate);

            var diff = (m_Point.position - m_Controller.m_Ship.transform.position);
            var mag = diff.magnitude;
            
            if (m_Vertical)
            {
                var m = 1f;
                if (diff.y > 0)
                {
                    m = -m;
                }
                m_Controller.m_Ship.transform.Rotate(m_Controller.m_Ship.transform.right, mag / m);
            }
            else
            {
                var m = 3f;
                if (diff.x > 0)
                {
                    m = -m;
                }
                m_Controller.m_Ship.transform.Rotate(m_Controller.m_Ship.transform.forward, mag / m);
                //m_Controller.m_Ship.transform.Rotate(m_Controller.m_Ship.transform.up, mag / m * -1.2f);
            }


            if (mag <= 1f)
            {
                m_Controller.SetState(new Idle());
            }
        }
    }
}
