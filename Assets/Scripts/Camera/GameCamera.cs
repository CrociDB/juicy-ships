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
    }
}

