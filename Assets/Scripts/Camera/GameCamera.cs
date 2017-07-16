using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Ship
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField]
        private PostProcessingBehaviour m_PostProcessing;

        private void Awake()
        {
            m_PostProcessing = GetComponent<PostProcessingBehaviour>();
        }

        public void ToggleEffects()
        {
            m_PostProcessing.enabled = !m_PostProcessing.enabled;
        }
    }
}

