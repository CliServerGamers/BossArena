using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Boss.BossAbilities
{
    class TeleportToRandomGoop : Node
    {

        private GameObject boss;
        public TeleportToRandomGoop(GameObject boss) {
            this.boss = boss;
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
            Teleport();

            state = NodeState.SUCCESS;
            return NodeState.SUCCESS;
        }

        private void Teleport()
        {
            GameObject eod = GameObject.FindGameObjectWithTag("EOD");
            if (eod != null)
            {
                boss.transform.position = eod.transform.position;
            }
        }

    }
}
