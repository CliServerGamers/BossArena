using Assets.Scripts.Game.BehaviorTree;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
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
        private Boss boss;
        private GameObject attackOutline;

        private const float largeHitboxRadius = 5.0f;

        public SkyDive(GameObject boss, GameObject eodPrefab, GameObject shadowPrefab, GameObject attackOutline)
        {
            this.boss = boss.GetComponent<Boss>();
            this.eodPrefab = eodPrefab;
            this.isDiving = true;
            this.shadow = shadowPrefab;
            this.attackOutline = attackOutline;
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

            boss.GetComponent<Animator>().SetBool("isJumping", false);
            boss.GetComponent<Animator>().SetBool("isAttacking", true);

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
                ApplySmallHitboxDamage();
                ApplyLargeHitboxDamage();

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
            boss.PlaySound("vine-boom", 1.0f, 1.0f);
        }

        private void SpawnEOD()
        {
            // Dont spawn if not on server 
            if (!NetworkManager.Singleton.IsServer)
            {
                return;
            }

            GameObject eod = Boss.Instantiate(eodPrefab, boss.transform.position, boss.transform.rotation);
            eod.transform.position = new Vector2(eod.transform.position.x, eod.transform.position.y - 0.5f);
            eod.GetComponent<NetworkObject>().Spawn();
        }

        private void MoveToward(Transform follower, Transform target, float speed)
        {
            follower.position = Vector3.MoveTowards(follower.position, target.position, speed * Time.deltaTime);
        }

        private void ApplySmallHitboxDamage()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Renderer bossRenderer = boss.GetComponent<SpriteRenderer>();
            float bossRadius = Mathf.Sqrt(
                bossRenderer.bounds.size.x + bossRenderer.bounds.size.x *
                bossRenderer.bounds.size.y + bossRenderer.bounds.size.y);
            foreach (GameObject player in players)
            {

                if (IsWithinRange(player.transform.position, boss.transform.position, bossRadius))
                {
                    //player.GetComponent<Player>().CurrentHealth.Value -= smallHitBoxDamage;
                    //RPC call

                    if (player.GetComponent<Collider2D>().enabled == true)
                    {
                        player.GetComponent<Player>().TakeDamageClientRpc(boss.skydiveSmallHitBoxDamage);
                    }

                    Debug.Log("Blobbbed");
                }
            }
            //DisplayRadiusAroundPosition(boss.transform.position, bossRadius);
        }

        private void ApplyLargeHitboxDamage()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (IsWithinRange(player.transform.position, boss.transform.position, largeHitboxRadius))
                {
                    //player.GetComponent<Player>().CurrentHealth.Value -= largeHitBoxDamage;
                    //RPC call

                    if (player.GetComponent<Collider2D>().enabled == true)
                    {
                        player.GetComponent<Player>().TakeDamageClientRpc(boss.skydiveLargeHitBoxDamage);
                    }
                    Debug.Log("blorbsted");
                }
            }
            DisplayRadiusAroundPosition(boss.transform.position, largeHitboxRadius);
        }

        private bool IsWithinRange(Vector3 player, Vector3 boss, float radius)
        {
            float distancePlayerFromBoss = Vector3.Distance(boss, player);
            float distanceBossFromHitbox = Vector3.Distance(boss, boss + new Vector3(radius, radius, radius));
            return distancePlayerFromBoss < distanceBossFromHitbox;
        }

        private void DisplayRadiusAroundPosition(Vector3 position, float radius)
        {
            GameObject attackOutlineGameObject = Boss.Instantiate(attackOutline, position, boss.transform.rotation);
            attackOutlineGameObject.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(radius/2, radius/2, radius/2);           
        }

    }
}