using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

using DG.Tweening;

namespace Ship
{
    [RequireComponent(typeof(PostProcessingBehaviour))]
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private FadeEffect m_FadeEffect;

        private PostProcessingBehaviour m_PostProcessing;
        private Camera m_Camera;

        private float m_DefaultFoV;
        private Color m_DefaultColor;

        private Tween m_FoVTween;

        private void Awake()
        {
            m_PostProcessing = GetComponent<PostProcessingBehaviour>();
            m_Camera = GetComponent<Camera>();

            m_DefaultFoV = m_Camera.fieldOfView;
            m_DefaultColor = m_Camera.backgroundColor;

            m_FadeEffect.ShowAll();

            m_PostProcessing.enabled = PlayerPrefs.GetInt("PostEffects", 0) == 1;
        }

        // Change values

        public void SetFoV(float fov)
        {
            m_Camera.fieldOfView = fov;
        }

        public void SetBackgroundColor(Color color)
        {
            m_Camera.backgroundColor = color;
            RenderSettings.fogColor = color;
        }

        // Effects

        public void ToggleEffects()
        {
            m_PostProcessing.enabled = !m_PostProcessing.enabled;
            PlayerPrefs.SetInt("PostEffects", Convert.ToInt32(m_PostProcessing.enabled));
        }

        public void StartDashing()
        {
            if (m_FoVTween != null)
            {
                m_FoVTween.Kill();
            }

            m_FoVTween = m_Camera.DOFieldOfView(75f, .08f).SetEase(Ease.Linear);
        }

        public void StopDashing()
        {
            if (m_FoVTween != null)
            {
                m_FoVTween.Kill();
            }

            m_FoVTween = m_Camera.DOFieldOfView(m_DefaultFoV, .1f).SetEase(Ease.Linear);
        }

        public void ShakeDestroy()
        {
            m_Camera.DOShakePosition(.2f, .5f, 100, 220);
        }

        public void FadeIn(float time)
        {
            m_FadeEffect.FadeIn(time);
        }

        public void FadeOut(float time)
        {
            m_FadeEffect.FadeOut(time);
        }
    }
}

