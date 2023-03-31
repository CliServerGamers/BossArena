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
        private AbilityTimer<int, int> jumpTimer;
        private bool isJumping;
        private const float totalJumpTime = 5.0f;
        private const int maxHeight = 10;
        private const float jumpSpeed = 15f;

        // player that is being targeted: TODO: Change this so that it is set in a decorator node and fecthed from that node in this one
        private GameObject player;

        private GameObject shadow;
        private GameObject boss;

        public BossExitScreen(GameObject boss, GameObject shadowPrefab)
        {
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            this.boss = boss;
            this.shadow = shadowPrefab;
            this.isJumping = false;
            this.jumpTimer = new AbilityTimer<int, int>(totalJumpTime, AfterCountDown);
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

 
            if (!isJumping)
            {
                SetupJump();
                this.jumpTimer.Restart();
                this.jumpTimer.Run();
            } else
            {
                this.jumpTimer.Update();
                MoveBossToJumpHeightLocation();
                MoveShadow();
            }
            return state;
        }

        private void SetupJump()
        {
            BoxCollider2D bossCollider = boss.transform.GetComponent<BoxCollider2D>();
            bossCollider.enabled = false;
            shadow.GetComponent<SpriteRenderer>().enabled = true;
            isJumping = true;
        }

        private void MoveBossToJumpHeightLocation()
        {
            float distance = maxHeight - boss.transform.position.y;
            if (distance > 0)
            {
                float movement = Mathf.Min(jumpSpeed * Time.deltaTime, distance);
                boss.transform.Translate(Vector3.up * movement);
            }
        }

        private int AfterCountDown(int dummy)
        {
            isJumping = false;
            state = NodeState.SUCCESS;
            return dummy;
        }
        
        private void MoveShadow()
        {
            if (jumpTimer.GetCurrentTime() >= 0.25f)
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

    }
}
