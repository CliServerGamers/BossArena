using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss;
using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using Node = Assets.Scripts.Game.BehaviorTree.Node;

namespace BossArena.game
{
    public class JumpAttack : Node
    {
        private enum JumpState
        {
            DEFAULT, START, UPWARDS, DIVE, LANDED
        }
        private JumpState jumpState;

        private const float diveSpeed = 100.0f;
        private const float totalJumpTime = 5.0f;
        private const float totalIdleTime = 2.0f;
        private const float shadowOffset = 2.0f;
        private const int maxHeight = 10;
        private float jumpTime;
        private float idleTime;
        private bool isEodSpawned;
        private bool isShadowEnabled;
        private float originalJumpY;

        // player that is being targeted: TODO: Change this so that it is set in a decorator node and fecthed from that node in this one
        private GameObject player;

        // used to Instantiate and Destroy shadow prefabs
        private GameObject shadowPrefab;

        private GameObject eod;

        // the current shadow gameobject
        private GameObject shadow;

        private GameObject boss;

        private Transform bossTransform;

        public JumpAttack(GameObject boss, GameObject eod, GameObject shadow)
        {
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            this.boss = boss;
            this.bossTransform = this.boss.transform;
            this.isShadowEnabled = false;
            this.eod = eod;
            this.isEodSpawned = false;
            this.shadowPrefab = shadow;
            jumpState = JumpState.START;
        }
        

        public override NodeState Evaluate()
        {
            switch (jumpState)
            {
                case JumpState.START:
                    SetupJump();
                    break;
                case JumpState.UPWARDS:
                    MoveBossToJumpHeightLocation();
                    RunCountDown();
                    MoveShadow();
                    break;
                case JumpState.DIVE:
                    MoveToward(bossTransform, shadow.transform, diveSpeed);

                    // finish the dive if the boss is close enough to its shadow
                    if (Vector3.Distance(shadow.transform.position, bossTransform.position) < 0.2f)
                    {
                        boss.transform.position = shadow.transform.position;
                        FinishJump();
                    }
                    break;
                case JumpState.LANDED:
                    if (!isEodSpawned)
                    {
                        SpawnEOD();
                        isEodSpawned = true;
                    }
                    RunIdleCountDown();
                    return state;
                default:
                    return state;
            }
            state = NodeState.RUNNING;
            return NodeState.RUNNING;
        }

        private void SetupJump()
        {
            BoxCollider2D bossCollider = bossTransform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = false;
            jumpTime = totalJumpTime;
            idleTime = totalIdleTime;
            originalJumpY = bossTransform.position.y;
            isEodSpawned = false;
            jumpState = JumpState.UPWARDS;
        }

        private void MoveBossToJumpHeightLocation()
        {
            float distance = maxHeight - bossTransform.position.y;
            if (distance > 0)
            {
                float movement = Mathf.Min(Boss.speed * Time.deltaTime, distance);
                bossTransform.Translate(Vector3.up * movement);
            }
        }

        private void RunCountDown()
        {
            jumpTime -= Time.deltaTime;
            if (jumpTime <= 0)
            {
                jumpState = JumpState.DIVE;
            }
        }

        private void RunIdleCountDown()
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
            {
                jumpState = JumpState.START;
                state = NodeState.SUCCESS;
            }
        }

        private void MoveShadow()
        {
            if (!isShadowEnabled && IsOffScreen())
            {
                shadow = Boss.Instantiate(shadowPrefab, Vector3.zero, Quaternion.identity);
                Debug.Log("Shadow here");
                isShadowEnabled = true;
            }

            if (jumpTime >= 0.25f && isShadowEnabled)
            {
                // have shadow target and follow the player
                const float shadowSpeed = 100;
                MoveToward(shadow.transform, player.transform, shadowSpeed);
            }
        }

        private void FinishJump()
        {
            BoxCollider2D bossCollider = bossTransform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = true;
            Boss.Destroy(shadow);
            jumpState = JumpState.LANDED;
            isShadowEnabled = false;
        }

        private void SpawnEOD()
        {
            Boss.Instantiate(eod, bossTransform.position, Quaternion.identity);
        }

        private void MoveToward(Transform follower, Transform target, float speed)
        {
            follower.position = Vector3.MoveTowards(follower.position, target.position, speed * Time.deltaTime);
        }

        private bool IsOffScreen()
        {
            return originalJumpY + shadowOffset < bossTransform.position.y;
        }

    }
}