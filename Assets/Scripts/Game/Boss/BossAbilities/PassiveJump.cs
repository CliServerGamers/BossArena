using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace BossArena.game
{
    enum PassiveJumpState
    {
        START, JUMPING, END
    }

    public class PassiveJump : Node
    {
        private PassiveJumpState passiveJumpState;
        private Vector3 initialCoords;
        private Vector3 finalCoords;
        private float jumpSpeed = 15f;

        private GameObject boss;
        private GameObject shadow;
        private GameObject player;

        public PassiveJump(GameObject boss, GameObject shadow)
        {
            this.boss = boss;
            this.shadow = shadow;
            this.player = GameObject.Find("PlayerPrefab(Clone)");
            passiveJumpState = PassiveJumpState.START;
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
            boss.GetComponent<Animator>().SetBool("isJumping", true);
            boss.GetComponent<Animator>().SetBool("isAttacking", false);

            switch (passiveJumpState)
            {
                case PassiveJumpState.START:
                    InitializeTrajectory();
                    break;
                case PassiveJumpState.JUMPING:
                    Jump();
                    break;
                case PassiveJumpState.END:
                    state = NodeState.SUCCESS;
                    passiveJumpState = PassiveJumpState.START;
                    return state;
            }
            return state;
        }

        private void InitializeTrajectory()
        {
            initialCoords = boss.transform.position;
            finalCoords = boss.GetComponent<Enemy>().CurrentTarget.transform.position;
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
                passiveJumpState = PassiveJumpState.END;
                boss.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

    }
}
