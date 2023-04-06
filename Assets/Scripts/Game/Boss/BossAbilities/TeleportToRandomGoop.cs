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
            // teleport to random eod in the scene if any
            GameObject[] eods = GameObject.FindGameObjectsWithTag("EOD");
            if (eods != null && eods.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, eods.Length);
                boss.transform.position = eods[randomIndex].transform.position;
            }
        }

    }
}
