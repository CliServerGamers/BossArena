using Assets.Scripts.Game.BehaviorTree;
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
            SelectTarget();


            // when done, set state to success
            state = NodeState.SUCCESS;
            return state;
        }

        void SelectTarget()
        {
            if (thisEnemy.State.Value == EntityState.TAUNTED) return;
            /// Create collision box with threatRadius
            /// Get players overlapped with collision
            Collider2D[] hitCol = Physics2D.OverlapCircleAll((Vector2)thisEnemy.transform.position, thisEnemy.threatRadius);
            Debug.Log($"Found {hitCol.Length}");
            foreach (Collider2D col in hitCol)
            {
                if (col.gameObject.TryGetComponent(out Player player))
                {
                    if (thisEnemy.CurrentTarget == null) thisEnemy.CurrentTarget = player;

                    if (thisEnemy.CurrentTarget.ThreatLevel.Value < player.ThreatLevel.Value)
                    {
                        Debug.Log($"Current Target threat level {thisEnemy.CurrentTarget.ThreatLevel.Value} : Detected Threat level {player.ThreatLevel.Value}");
                        thisEnemy.CurrentTarget = player;
                    }
                    Debug.Log($"Current Target {thisEnemy.CurrentTarget}");
                }
            }
        }
    }
}
