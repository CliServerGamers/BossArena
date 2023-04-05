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
        private Enemy thieEnemy;
        public TargetSelectionNode(GameObject boss)
        {
            this.thieEnemy = boss.GetComponent<Enemy>();
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
            SelectTarget();


            // when done, set state to success
            return NodeState.SUCCESS;
        }

        void SelectTarget()
        {
            /// Create collision box with threatRadius
            /// Get players overlapped with collision
            Collider2D[] hitCol = Physics2D.OverlapCircleAll((Vector2)thieEnemy.transform.position, thieEnemy.threatRadius);
            foreach (Collider2D col in hitCol)
            {
                if (col.gameObject.TryGetComponent(out Player player))
                {
                    if(thieEnemy.CurrentTarget == null) thieEnemy.CurrentTarget = player;

                    if (thieEnemy.CurrentTarget.ThreatLevel.Value < player.ThreatLevel.Value)
                    {
                        Debug.Log($"Current Target threat level {thieEnemy.CurrentTarget.ThreatLevel.Value} : Detected Threat level {player.ThreatLevel.Value}");
                        thieEnemy.CurrentTarget = player;
                    }
                    Debug.Log($"Current Target {thieEnemy.CurrentTarget}");
                }
            }
            //var tempMonoArray = col.gameObject.GetComponent<Player>();
            //foreach (var monoBehaviour in tempMonoArray)
            //{
            //    if (monoBehaviour is Player player)
            //    {

            //    }
            //}

            /// Get player with highest threat
            /// Set target
        }
    }
}
