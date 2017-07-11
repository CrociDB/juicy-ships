using SectionControl;
using Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public ShipController m_ShipController;
        public SectionManager m_SectionManager;

        void Start()
        {
            StartCoroutine(InitGame());
        }

        private IEnumerator InitGame()
        {
            yield return new WaitForEndOfFrame();
            m_ShipController.Init();
            m_SectionManager.Init(m_ShipController);
        }
    }
}
