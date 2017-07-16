using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ship;

namespace Entities
{
    public class Enemy : Entity
    {
        protected override void TouchedPlayer(ShipObject shipObject)
        {
            base.TouchedPlayer(shipObject);

            var player = shipObject.m_Controller;

            if (player.Dashing)
            {
                player.DestroyedEnemy();
            }
        }
    }
}
