using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.States
{
    public class Pregame : State
    {
        public GameManager m_Manager;

        public override void Enter()
        {
            base.Enter();
            m_Manager = (GameManager)m_Entity;
            m_Manager.StartCoroutine(PregameFlow());
        }

        private IEnumerator PregameFlow()
        {
            m_Manager.Camera.FadeOut(1.0f);
            yield return new WaitForSeconds(1f);

            m_Manager.Level.SetNextLevel();
            m_Manager.UIManager.LevelStart(m_Manager.Level.CurrentLevel);
            yield return new WaitForSeconds(1.5f);
            m_Manager.UIManager.HideAll();
            yield return new WaitForSeconds(.1f);
            m_Manager.SetState(new Playing());
        }
    }
}
