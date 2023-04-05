using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class ProjectileAttackNode: Node
    {

        private float timeToLive = 5.0f;
        private GameObject projectilePrefab;
        private GameObject boss;
        private float amount;
        private float angleOffset;
        private float swirlOffset;

        public ProjectileAttackNode(GameObject boss, GameObject projectilePrefab, int amount, float swirlOffset, float angleOffset) {
            this.projectilePrefab = projectilePrefab;
            this.boss = boss;
            this.amount = amount;
            this.angleOffset = angleOffset;
            this.swirlOffset = swirlOffset;
        }

        public override NodeState Evaluate()
        {
            if (state == NodeState.READY)
            {
                state = NodeState.RUNNING;

                SpawnProjectilesAroundBoss();

                state = NodeState.SUCCESS;
            }
            return state;
        }

        private void SpawnProjectilesAroundBoss()
        {
            SpriteRenderer renderer = boss.GetComponent<SpriteRenderer>();
            float radius = Mathf.Sqrt(renderer.bounds.size.x * renderer.bounds.size.x + renderer.bounds.size.y * renderer.bounds.size.y);
            float angleStep = (Mathf.PI * 2) / amount + (angleOffset / 360);
            for (int i = 0; i < amount; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle + swirlOffset) * radius;
                float y = Mathf.Sin(angle + swirlOffset) * radius;
                Vector3 spawnPosition = boss.transform.position + new Vector3(x, y, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
                Boss.Instantiate(projectilePrefab, spawnPosition, rotation);
            }
        }

    }
}
