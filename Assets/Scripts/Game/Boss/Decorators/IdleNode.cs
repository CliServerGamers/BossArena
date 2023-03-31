using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace BossArena.game
{
    public class IdleNode : Node
    {

        private float idleTime;
        AbilityTimer<int, int> idleTimer;

        public IdleNode(float idleTime) {
            this.idleTimer = new AbilityTimer<int, int>(idleTime, AfterTimer);
            this.idleTime= idleTime;
        }

        public override NodeState Evaluate()
        {
            if (state == NodeState.READY)
            {
                idleTimer.Restart();
                state = NodeState.RUNNING;
            }

            if (state != NodeState.RUNNING)
            {
                return state;
            }

            // if we are in the ready state, set the node state to running
            idleTimer.Update();
            return state;
        }

        private int AfterTimer(int dummy)
        {
            state = NodeState.SUCCESS;
            return dummy;
        }

    }

}