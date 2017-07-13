using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using DG.Tweening;

using SimpleStateMachine;

namespace Ship
{
    public class ShipObject : GameEntity
    {
        public Action<Collider> OnCollide;

        private Sequence m_BumpSequence;

        private void OnTriggerEnter(Collider other)
        {
            if (OnCollide != null)
            {
                OnCollide.Invoke(other);
            }

            BumpAnim();
        }

        private void BumpAnim()
        {
            if (m_BumpSequence != null)
            {
                m_BumpSequence.Kill();
            }

            m_BumpSequence = DOTween.Sequence();
            m_BumpSequence.Append(transform.DOScale(1.05f, .05f)).Append(transform.DOScale(1f, .05f));
        }
    }
}
