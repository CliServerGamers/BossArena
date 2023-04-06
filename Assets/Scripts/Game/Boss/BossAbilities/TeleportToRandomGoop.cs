using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    class TeleportToRandomGoop : Node
    {

        private Boss boss;
        public TeleportToRandomGoop(GameObject boss) {
            this.boss = boss.GetComponent<Boss>();
        }

        public override NodeState Evaluate()
        {
            if (state == NodeState.READY)
            {
                state = NodeState.RUNNING;
            }

            if (state != NodeState.RUNNING)
            {
                return state;
            }

            Debug.Log("About to teleport!");
            boss.Teleport();

            state = NodeState.SUCCESS;
            return NodeState.SUCCESS;
        }

      

    }
}
