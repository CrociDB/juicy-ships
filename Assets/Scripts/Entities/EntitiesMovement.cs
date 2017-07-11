using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class EntitiesMovement : MonoBehaviour
    {
        public float m_SpeedRotation = .1f;
        public float m_SpeedMovement = 5;
        public float m_AmountMovement = .1f;

        private void Update()
        {
            transform.localPosition = new Vector3(
                0f,
                Mathf.Cos(Time.time * m_SpeedMovement) * m_AmountMovement,
                0f);

            transform.Rotate(transform.up, (m_SpeedRotation * Time.deltaTime));
        }
    }
}
