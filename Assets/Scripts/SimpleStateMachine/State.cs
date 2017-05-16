using UnityEngine;
using System.Collections;

namespace SimpleStateMachine
{
	public class State 
	{
		protected GameEntity m_Entity;

		public State()
		{
		}

        public void SetEntity(GameEntity entity)
        {
            m_Entity = entity;
        }

		public virtual void Enter()
		{
		}

		public virtual void Update()
		{
		}

        public virtual void FixedUpdate()
        {
        }

        public virtual void LateUpdate()
        {
        }

		public virtual void Exit()
		{
		}
	}
}
