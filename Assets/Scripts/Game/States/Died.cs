using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.States
{
    public class Died : State
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
            m_Manager.m_LevelManager.m_Speed = 0f;
            m_Manager.m_ShipController.Stop();
            yield return new WaitForSeconds(.1f);
            m_Manager.Camera.FadeOut(1f);
            yield return new WaitForSeconds(1f);
            m_Manager.LeaveGame();
        }
    }
}
