using Assets.Scripts.Game.BehaviorTree;
using UnityEngine;

using Node = Assets.Scripts.Game.BehaviorTree.Node;

namespace BossArena.game
{
    public class SkyDive : Node
    {
        private const float diveSpeed = 100.0f;
        private bool isDiving;

        private GameObject eodPrefab;
        private GameObject shadow;
        private GameObject boss;

        public SkyDive(GameObject boss, GameObject eodPrefab, GameObject shadowPrefab)
        {
            this.boss = boss;
            this.eodPrefab = eodPrefab;
            this.isDiving = true;
            this.shadow = shadowPrefab;
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

            if (isDiving)
            {
                MoveToward(boss.transform, shadow.transform, diveSpeed);

                // finish the dive if the boss is close enough to its shadow
                if (Vector3.Distance(boss.transform.position, shadow.transform.position) < 0.2f)
                {
                    FinishJump();
                    isDiving = false;
                }
            } else
            {
                SpawnEOD();
                state = NodeState.SUCCESS;
                isDiving = true;
            }

            return state;
        }

        private void FinishJump()
        {
            BoxCollider2D bossCollider = boss.transform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = true;
            shadow.GetComponent<SpriteRenderer>().enabled = false;
        }

        private void SpawnEOD()
        {
            Boss.Instantiate(eodPrefab, boss.transform.position, Quaternion.identity);
        }

        private void MoveToward(Transform follower, Transform target, float speed)
        {
            follower.position = Vector3.MoveTowards(follower.position, target.position, speed * Time.deltaTime);
        }

    }
}