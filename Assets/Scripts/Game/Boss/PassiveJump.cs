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
        private PassiveJumpState passiveJumpState;
        private Vector3 initialCoords;
        private Vector3 finalCoords;

        private GameObject boss;
        private GameObject shadow;
        private GameObject player;

        private AbilityTimer idleTimer;

        public PassiveJump(GameObject boss, GameObject shadow)
        {
            this.boss = boss;
            this.shadow = shadow;
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            passiveJumpState = PassiveJumpState.START;

            const float idleTime = 3;
            idleTimer = new AbilityTimer(idleTime, AfterIdleTimer);
        }

        public override NodeState Evaluate()
        {
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
                    passiveJumpState = PassiveJumpState.START;
                    state = NodeState.SUCCESS;
                    return state;
            }
            state = NodeState.RUNNING;
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
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, finalCoords, Boss.speed * Time.deltaTime);
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
