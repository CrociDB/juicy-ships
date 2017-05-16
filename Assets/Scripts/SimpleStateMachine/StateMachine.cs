using UnityEngine;
using System.Collections;

namespace SimpleStateMachine
{
	public class StateMachine : MonoBehaviour 
	{
        protected GameEntity m_Entity;

		protected State m_CurrentState;
		protected State m_LastState;

        public void SetEntity(GameEntity entity)
        {
            m_Entity = entity;
        }

        public void SetState(State newState)
        {
            m_LastState = m_CurrentState;
            
            if (m_CurrentState != null)
            {
                m_CurrentState.Exit();
            }

            m_CurrentState = newState;

            if (newState != null)
            {
                m_CurrentState.SetEntity(m_Entity);
                m_CurrentState.Enter();
            }
        }

        void Update()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.Update();
            }
        }

        void FixedUpdate()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.FixedUpdate();
            }
        }

        void LateUpdate()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.LateUpdate();
            }
        }
	}
}
