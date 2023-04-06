using Assets.Scripts.Game.BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class GenericEnemy : EntityBase, IHostile
    {
        protected override void HandleCollision(Collision2D collision)
        {
            throw new System.NotImplementedException();
        }

        //protected override Node SetupTree()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
