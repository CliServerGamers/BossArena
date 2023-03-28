using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BossArena.game;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using Unity.VisualScripting;

namespace BossArena.game
{
    public class JumpAttack: Node
    {
        private const float totalJumpTime = 5.0f;
        private float jumpTime;
        private const float shadowOffset = 2.0f;
        private const int maxHeight = 10;
        private bool isShadowSpawned;
        private float originalJumpY;
        private bool isJumping = false;
        private bool isJumpCountDownRunning = false;

        private GameObject player; 
        private GameObject shadowPrefab;
        private GameObject shadow;
        private Transform bossTransform;
 
        public JumpAttack(Boss boss, GameObject shadow) {
            this.bossTransform = boss.transform;
            this.shadowPrefab = shadow;
            this.isShadowSpawned = false;
            this.player = GameObject.Find("PlayerPrefab(Clone)");
        }

        public override NodeState Evaluate()
        {
            // set up for jump
            if (!isJumping)
            {
                SetupJump();
            } 
            
            // move the boss up to its jump height location if the jump count down is running and if its not there yet
            // if the jump timer is running still, keep running down the clock
            if (isJumpCountDownRunning)
            {
                MoveBossToJumpHeightLocation();
                RunCountDown();
            }

            // drop the boss from the sky and have it fall onto it's shadow
            if (!isJumpCountDownRunning)
            {
                const float diveSpeed = 100f;
                MoveToward(bossTransform, shadow.transform, diveSpeed);

                // finish the jump if the boss is close enough to its shadow
                if (Vector3.Distance(shadow.transform.position, bossTransform.position) < 0.2f)
                {
                    FinishJump();
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            if (jumpTime >= 0.5 && isShadowSpawned)
            {
                // have shadow target and follow the player
                const float shadowSpeed = 100;
                MoveToward(shadow.transform, player.transform, shadowSpeed);
            }

            // check to see if we should spawn the shadow
            if (!isShadowSpawned && IsOffScreen())
            {
                shadow = Boss.Instantiate(shadowPrefab, Vector3.zero, Quaternion.identity);
                Debug.Log("Shadow here");
                isShadowSpawned = true;
            }
            
            state = NodeState.RUNNING;
            return state;
        }

        private void SetupJump()
        {
            isJumping = true;
            isJumpCountDownRunning = true;
            BoxCollider2D bossCollider = bossTransform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = false;
            jumpTime = totalJumpTime;
            originalJumpY = bossTransform.position.y;
        }

        private void FinishJump()
        {
            BoxCollider2D bossCollider = bossTransform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = true;
            isJumping = false;
            isShadowSpawned = false;
            Boss.Destroy(shadow);
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

        private void MoveToward(Transform follower, Transform target, float speed)
        {
            follower.position = Vector3.MoveTowards(follower.position, target.position, speed * Time.deltaTime);
        }

        private void RunCountDown()
        {
            jumpTime -= Time.deltaTime;

            if (jumpTime <= 0)
            {
                isJumpCountDownRunning = false;
            }
        }

        private bool IsOffScreen()
        {
            return originalJumpY + shadowOffset < bossTransform.position.y;
        }

    }
}