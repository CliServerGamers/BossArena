using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    class TargetSelectionNode : Node
    {
        private Enemy thisEnemy;
        public TargetSelectionNode(GameObject boss)
        {
            this.thisEnemy = boss.GetComponent<Enemy>();
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
            Debug.Log($"TARGETING");
            thisEnemy.SelectTarget();


            // when done, set state to success
            state = NodeState.SUCCESS;
            return state;
        }

       
    }
}
