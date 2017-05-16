using UnityEngine;
using System.Collections;

namespace SimpleStateMachine
{
    public class GameEntity : MonoBehaviour 
    {
        protected StateMachine m_StateMachine;

        public GameEntity()
        {
        }

	    protected virtual void Start () 
        {
            m_StateMachine = gameObject.AddComponent<StateMachine>();
            m_StateMachine.SetEntity(this);
            m_StateMachine.SetState(new State());
	    }

        public void SetState(State newState)
        {
            m_StateMachine.SetState(newState);
        }
    }
}
