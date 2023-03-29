using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss;
using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace BossArena.game
{
    public class JumpAttackAbility : NetworkBehaviour, IBossAbility
    {
        private enum JumpState
        {
            DEFAULT, START, UPWARDS, DIVE, LANDED
        }
        private JumpState jumpState;

        private const float diveSpeed = 100.0f;
        private const float totalJumpTime = 5.0f;
        private const float shadowOffset = 2.0f;
        private const int maxHeight = 10;
        private float jumpTime;
        private bool isShadowEnabled;
        private float originalJumpY;

        // player that is being targeted: TODO: Change this so that it is set in a decorator node and fecthed from that node in this one
        private GameObject player;

        // used to Instantiate and Destroy shadow prefabs
        [SerializeField]
        private GameObject shadowPrefab;

        [SerializeField]
        private GameObject eod;

        // the current shadow gameobject
        private GameObject shadow;

        private GameObject boss;

        private Transform bossTransform;

        [SerializeField]
        private Node correspondingNode;

        public void InitializeNode(Node node)
        {
            correspondingNode = node;
            this.boss = correspondingNode.boss;
            this.bossTransform = this.boss.transform;
            jumpState = JumpState.START;
        }

        protected void Start()
        {
            this.isShadowEnabled = false;
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            jumpState = JumpState.START;
        }

        private NodeState state;

        public NodeState RunAbility()
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
                        FinishJump();
                    }
                    break;
                case JumpState.LANDED:
                    SpawnEOD();
                    jumpState = JumpState.START;
                    state = NodeState.SUCCESS;
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
            originalJumpY = bossTransform.position.y;
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
