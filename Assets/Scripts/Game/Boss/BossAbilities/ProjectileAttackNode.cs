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
        public ProjectileAttackNode(GameObject boss, GameObject projectilePrefab) {
            this.projectilePrefab = projectilePrefab;
            this.boss = boss;
        }

        public override NodeState Evaluate()
        {
            if (state == NodeState.READY)
            {
                state = NodeState.RUNNING;

                SpawnProjectilesAroundBoss(8);

                state = NodeState.SUCCESS;
            }
            return state;
        }

        private void SpawnProjectilesAroundBoss(int amount)
        {
            LinkedList<GameObject> list = new LinkedList<GameObject>();
            SpriteRenderer renderer = boss.GetComponent<SpriteRenderer>();
            float radius = Mathf.Sqrt(renderer.bounds.size.x * renderer.bounds.size.x + renderer.bounds.size.y * renderer.bounds.size.y);
            float angleStep = Mathf.PI * 2 / amount;
            for (int i = 0; i < amount; i++)
            {
                float angle = i * angleStep;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                Vector3 spawnPosition = boss.transform.position + new Vector3(x, y, 0);
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);
                GameObject newProjectile = Boss.Instantiate(projectilePrefab, spawnPosition, rotation);
                list.AddLast(newProjectile);
            }
        }

    }
}
