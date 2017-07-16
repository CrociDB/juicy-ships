using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using SimpleStateMachine;
using Game;

namespace Ship
{
    [System.Serializable]
    class ShipLineWaypoints
    {
        public Transform[] m_Waypoints;
    }

    public class ShipController : GameEntity
    {
        private GameManager m_GameManager;

        [Header("Settings")]
        [SerializeField]
        internal ShipLineWaypoints[] m_Waypoints;

        internal int m_CurrentWaypointX;
        internal int m_CurrentWaypointY;

        [SerializeField]
        internal Transform[] m_Lines;

        [SerializeField]
        internal ShipObject m_Ship;

        [Header("Game Settings")]
        public float m_LerpRate = 8f;
        public float m_TimeInDash = .5f;

        public GameManager GameManager
        {
            get
            {
                return m_GameManager;
            }
        }

        public bool Dashing
        {
            get
            {
                return GetState() is Dashing;
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        public void Init(GameManager gameManager)
        {
            m_GameManager = gameManager;
            m_CurrentWaypointX = 1;
            SetState(new Idle());
        }

        internal Transform GetLeft()
        {
            if (m_CurrentWaypointX == 0)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY].m_Waypoints[m_CurrentWaypointX - 1];
        }

        internal Transform GetRight()
        {
            if (m_CurrentWaypointX == m_Waypoints[m_CurrentWaypointY].m_Waypoints.Length - 1)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY].m_Waypoints[m_CurrentWaypointX + 1];
        }

        internal Transform GetUp()
        {
            if (m_CurrentWaypointY == m_Waypoints.Length - 1)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY + 1].m_Waypoints[m_CurrentWaypointX];
        }

        internal Transform GetDown()
        {
            if (m_CurrentWaypointY == 0)
            {
                return null;
            }

            return m_Waypoints[m_CurrentWaypointY - 1].m_Waypoints[m_CurrentWaypointX];
        }

        internal void DestroyedEnemy()
        {
            m_GameManager.Camera.ShakeDestroy();
        }

        void Update()
        {

        }
    }
}
