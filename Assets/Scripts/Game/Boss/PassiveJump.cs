using Assets.Scripts.Game.BehaviorTree;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
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

        public PassiveJump()
        {
            passiveJumpState = PassiveJumpState.START;
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
                case PassiveJumpState.END:
                    End();
                    passiveJumpState = PassiveJumpState.START;
                    return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }

        private void InitializeTrajectory()
        {
            // TODO: set the initialCoords to the boss's position
            // TODO: set the finalCoords to the targeted player coords
            // TODO: turn hitbox off
        }

        private void Jump()
        {
            // TODO: have the boss follow the trajectory along with its shadow and animate it
        }

        private void End()
        {
            // TODO: turn hitbox on again
        }

    }
}
