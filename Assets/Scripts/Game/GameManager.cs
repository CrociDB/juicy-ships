using SectionControl;
using Ship;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PostProcessing;

using DG.Tweening;
using UnityEngine.SceneManagement;
using Game.UI;
using SimpleStateMachine;
using Game.States;

namespace Game
{
    public class GameManager : GameEntity
    {
        public ShipController m_ShipController;
        public LevelManager m_LevelManager;
        public UIManager m_UIManager;

        [SerializeField]
        private GameCamera m_Camera;

        public GameCamera Camera
        {
            get
            {
                return m_Camera;
            }
        }

        public LevelManager Level
        {
            get
            {
                return m_LevelManager;
            }
        }

        public UIManager UIManager
        {
            get
            {
                return m_UIManager;
            }
        }

        protected override void Start()
        {
            base.Start();
            StartCoroutine(InitGame());
        }

        private IEnumerator InitGame()
        {
            yield return new WaitForEndOfFrame();
            m_ShipController.Init(this);
            m_LevelManager.Init(m_ShipController, OnReachEndOfLevel);
            m_UIManager.Init();

            SetState(new Pregame());
        }

        private void OnReachEndOfLevel()
        {
            SetState(new Postgame());
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        private IEnumerator GameOverRoutine()
        {
            m_LevelManager.m_Speed = 0f;
            yield return new WaitForSeconds(.1f);
            m_Camera.FadeOut(1f);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Menu");
        }

        internal void PlayGame()
        {
            m_ShipController.PlayGame();
            m_LevelManager.PlayGame();
        }
    }
}
