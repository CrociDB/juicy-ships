using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class Playing : State
    {
        public GameManager m_Manager;

        public override void Enter()
        {
            base.Enter();
            m_Manager = (GameManager)m_Entity;
            m_Manager.PlayGame();
            m_Manager.Camera.FadeIn(1.0f);
        }
    }
}
