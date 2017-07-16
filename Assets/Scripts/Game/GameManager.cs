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

        [SerializeField]
        private GameCamera m_Camera;

        public GameCamera Camera
        {
            get
            {
                return m_Camera;
            }
        }

        void Start()
        {
            StartCoroutine(InitGame());
        }

        private IEnumerator InitGame()
        {
            yield return new WaitForEndOfFrame();
            m_ShipController.Init(this);
            m_SectionManager.Init(m_ShipController);
        }
    }
}
