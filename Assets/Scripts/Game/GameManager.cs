using SectionControl;
using Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PostProcessing;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public ShipController m_ShipController;
        public SectionManager m_SectionManager;

        public PostProcessingBehaviour m_PostProcessing;

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

        public void ToggleEffects()
        {
            m_PostProcessing.enabled = !m_PostProcessing.enabled;
        }
    }
}
