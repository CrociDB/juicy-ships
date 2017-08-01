using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        // Level Start
        public GameObject m_WinLevelStart;
        public Text m_TexLevelNumber;

        public void Awake()
        {
            Assert.IsNotNull(m_WinLevelStart, "Level Start window is null!");
            Assert.IsNotNull(m_TexLevelNumber, "Level Start level number is null!");
        }

        public void Init()
        {
        }

        // Window callings
        public void HideAll()
        {
            m_WinLevelStart.SetActive(false);
        }

        public void LevelStart(int levelNumber)
        {
            HideAll();

            m_TexLevelNumber.text = levelNumber.ToString();
            m_WinLevelStart.SetActive(true);
        }
    }
}
