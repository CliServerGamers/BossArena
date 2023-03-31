using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    public class BossExitScreen : Node
    {
        private AbilityTimer jumpTimer;
        private bool isJumping;
        private const float totalJumpTime = 5.0f;
        private const float shadowOffset = 2.0f;
        private const int maxHeight = 10;
        private float jumpTime;
        private bool isShadowEnabled;
        private float originalJumpY;

        // player that is being targeted: TODO: Change this so that it is set in a decorator node and fecthed from that node in this one
        private GameObject player;

        // used to Instantiate and Destroy shadow prefabs
        private GameObject shadowPrefab;

        private GameObject shadow;

        private GameObject boss;

        public BossExitScreen(GameObject boss, GameObject shadowPrefab)
        {
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            this.boss = boss;
            this.isShadowEnabled = false;
            this.shadowPrefab = shadowPrefab;
            this.isJumping = false;
            this.jumpTimer = new AbilityTimer(totalJumpTime, AfterCountDown);
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

            if (!isJumping)
            {
                SetupJump();
                isJumping = true;
            } else
            {
                MoveBossToJumpHeightLocation();
                jumpTimer.Tick(Time.deltaTime);
                MoveShadow();
            }
            return state;
        }

        private void SetupJump()
        {
            BoxCollider2D bossCollider = boss.transform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = false;
            jumpTime = totalJumpTime;
            originalJumpY = boss.transform.position.y;
        }

        private void MoveBossToJumpHeightLocation()
        {
            float distance = maxHeight - boss.transform.position.y;
            if (distance > 0)
            {
                float movement = Mathf.Min(Boss.speed * Time.deltaTime, distance);
                boss.transform.Translate(Vector3.up * movement);
            }
        }

        private void AfterCountDown()
        {
            isShadowEnabled = false;
            isJumping = false;
            state = NodeState.SUCCESS;
        }
        
        private void MoveShadow()
        {
            if (!isShadowEnabled && IsOffScreen())
            {
                shadow = Boss.Instantiate(shadowPrefab, player.transform.position, Quaternion.identity);
                shadow.GetComponent<SpriteRenderer>().enabled = true;
                isShadowEnabled = true;
            }

            if (jumpTime >= 0.25f && isShadowEnabled)
            {
                // have shadow target and follow the player
                const float shadowSpeed = 100;
                MoveToward(shadow.transform, player.transform, shadowSpeed);
            }
        }

        private void MoveToward(Transform follower, Transform target, float speed)
        {
            follower.position = Vector3.MoveTowards(follower.position, target.position, speed * Time.deltaTime);
        }

        private bool IsOffScreen()
        {
            return originalJumpY + shadowOffset < boss.transform.position.y;
        }

    }
}
