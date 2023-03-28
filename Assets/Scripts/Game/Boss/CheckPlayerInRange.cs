using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BossArena.game;

namespace Assets.Scripts.Game.Boss
{
    public class CheckPlayerInRange : Node
    {

        private Transform _transform;
        private static int _enemyLayerMask = 1 << 6;

        public CheckPlayerInRange(Transform transform)
        {
            this._transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    _transform.position, 5, _enemyLayerMask);

                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target", colliders[0].transform);

                    state = NodeState.SUCCESS;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }

    }
}
