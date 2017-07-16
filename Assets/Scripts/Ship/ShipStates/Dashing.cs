using DG.Tweening;
using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Dashing : Idle
    {
        private float m_Time;

        public override void Enter()
        {
            base.Enter();
            m_Time = m_Controller.m_TimeInDash;
            m_Controller.GameManager.Camera.StartDashing();
        }

        public override void Update()
        {
            base.Update();

            m_Time -= Time.deltaTime;
            if (m_Time <= 0)
            {
                m_Controller.SetState(new Idle());
            }
        }

        public override void Exit()
        {
            base.Exit();
            m_Controller.GameManager.Camera.StopDashing();
        }
    }
}
