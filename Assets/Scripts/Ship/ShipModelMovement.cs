using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class ShipModelMovement : MonoBehaviour
    {
        public float m_Speed = 5;
        public float m_AmountMovement = .1f;
        public float m_AmountRotation = .1f;

        private void Update()
        {
            transform.localPosition = new Vector3(
                0f,
                Mathf.Sin(Time.time * m_Speed) * m_AmountMovement,
                0f);

            transform.Rotate(transform.right, Mathf.Sin(Time.time * m_Speed) * m_AmountRotation);
        }
    }
}
