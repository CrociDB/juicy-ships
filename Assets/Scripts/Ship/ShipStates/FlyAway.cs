using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class FlyAway : State
    {
        protected ShipController m_Controller;

        private Vector3 m_Destiny;

        public override void Enter()
        {
            base.Enter();
            m_Controller = (ShipController)m_Entity;

            m_Destiny = m_Controller.m_Ship.transform.position + 
                (m_Controller.m_Ship.transform.forward * 100f);
        }

        public override void Update()
        {
            base.Update();
            m_Controller.m_Ship.transform.position = Vector3.Lerp(
                m_Controller.m_Ship.transform.position,
                m_Destiny,
                Time.deltaTime * m_Controller.m_LerpRate / 10f);
        }
    }
}
