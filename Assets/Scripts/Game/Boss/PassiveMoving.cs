using Assets.Scripts.Game.BehaviorTree;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
    public class PassiveMoving: Node
    {

        private Transform transform; 

        public PassiveMoving(Transform transform) {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            // move the boss to the right ever so slightly
            var pos = transform.position;
            pos.x += 0.001f;
            transform.position = pos;

            state = NodeState.RUNNING;
            return state;
        }
    }
}
