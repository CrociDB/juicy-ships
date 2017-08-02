using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.States
{
    public class Postgame : State
    {
        public GameManager m_Manager;

        public override void Enter()
        {
            base.Enter();
            m_Manager = (GameManager)m_Entity;
            m_Manager.StartCoroutine(PostgameFlow());
        }

        private IEnumerator PostgameFlow()
        {
            m_Manager.m_ShipController.FlyAway();
            yield return new WaitForSeconds(.5f);
            m_Manager.Camera.FadeOut(1.0f);
            yield return new WaitForSeconds(1f);
            m_Manager.SetState(new Pregame());
        }
    }
}
