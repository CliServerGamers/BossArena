using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BossArena.game
{
    enum PassiveJumpState
    {
        START, JUMPING, END, AFTER
    }

    public class PassiveJump : Node
    {
        private const float IDLE_TIME = 1.0f;

        private PassiveJumpState passiveJumpState;
        private Vector3 initialCoords;
        private Vector3 finalCoords;
        private float jumpSpeed = 15f;
        private const int AMOUNT_OF_JUMPS = 5;
        private int currentJumps;

        private GameObject boss;
        private GameObject shadow;
        private GameObject player;

        private AbilityTimer idleTimer;

        public PassiveJump(GameObject boss, GameObject shadow)
        {
            this.boss = boss;
            this.shadow = shadow;
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            this.currentJumps = 0;
            passiveJumpState = PassiveJumpState.START;
            idleTimer = new AbilityTimer(IDLE_TIME, AfterIdleTimer);
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

            switch (passiveJumpState)
            {
                case PassiveJumpState.START:
                    InitializeTrajectory();
                    break;
                case PassiveJumpState.JUMPING:
                    Jump();
                    break;
                case PassiveJumpState.AFTER:
                    idleTimer.Tick(Time.deltaTime);
                    break;
                case PassiveJumpState.END:
                    ++currentJumps;
                    if (currentJumps == AMOUNT_OF_JUMPS)
                    {
                        currentJumps = 0;
                        state = NodeState.SUCCESS;
                    }
                    passiveJumpState = PassiveJumpState.START;
                    return state;
            }
            return state;
        }

        private void InitializeTrajectory()
        {
            initialCoords = boss.transform.position;
            finalCoords = player.transform.position;
            boss.GetComponent<BoxCollider2D>().enabled = false;
            passiveJumpState = PassiveJumpState.JUMPING;
        }

        private void Jump()
        {
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, finalCoords, jumpSpeed * Time.deltaTime);
            const float basicallyZero = 0.01f;
            if (Vector3.Distance(boss.transform.position, finalCoords) < basicallyZero)
            {
                boss.transform.position = finalCoords;
                passiveJumpState = PassiveJumpState.AFTER;
                boss.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        public void AfterIdleTimer()
        {
            passiveJumpState = PassiveJumpState.END;
        }

    }
}
