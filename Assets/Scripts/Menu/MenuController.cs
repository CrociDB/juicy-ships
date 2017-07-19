using Ship;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private GameCamera m_Camera;

        private void Awake()
        {
            m_Camera.FadeIn(1f);
        }

        public void Play()
        {
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            m_Camera.FadeOut(.6f);
            yield return new WaitForSeconds(.7f);
            SceneManager.LoadScene("Game");
        }
    }
}
