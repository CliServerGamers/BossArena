using Assets.Scripts.Game.BehaviorTree;
using UnityEngine;

using Node = Assets.Scripts.Game.BehaviorTree.Node;

namespace BossArena.game
{
    public class SkyDive : Node
    {
        private const float diveSpeed = 100.0f;
        private float jumpTime;
        private bool isShadowEnabled;
        private bool isDiving;

        private GameObject eodPrefab;
        private GameObject shadowPrefab;
        private GameObject boss;

        private GameObject shadow;

        public SkyDive(GameObject boss, GameObject eodPrefab, GameObject shadowPrefab)
        {
            this.boss = boss;
            this.isShadowEnabled = false;
            this.eodPrefab = eodPrefab;
            this.isDiving = true;
            this.shadowPrefab = shadowPrefab;
            this.shadow = null;
        }
        

        public override NodeState Evaluate()
        {
            // if we are in the ready state, set the node state to running
            if (state == NodeState.READY)
            {
                state = NodeState.RUNNING;
            }
            // if we are not in the running state, dont do logic here
            if (state != NodeState.RUNNING)
            {
                return state;
            }

            if (isDiving)
            {
                if (shadow == null)
                {
                    shadow = GameObject.FindGameObjectWithTag("shadow");
                }

                MoveToward(boss.transform, shadow.transform, diveSpeed);

                // finish the dive if the boss is close enough to its shadow
                if (Vector3.Distance(shadow.transform.position, boss.transform.position) < 0.2f)
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

            Boss.Destroy(shadow);
            isShadowEnabled = false;
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