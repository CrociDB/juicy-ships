using SectionControl;
using Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PostProcessing;

using DG.Tweening;
using UnityEngine.SceneManagement;

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
            m_Camera.FadeIn(1f);
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        private IEnumerator GameOverRoutine()
        {
            m_SectionManager.m_Speed = 0f;
            yield return new WaitForSeconds(.1f);
            m_Camera.FadeOut(1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Menu");
        }
    }
}
