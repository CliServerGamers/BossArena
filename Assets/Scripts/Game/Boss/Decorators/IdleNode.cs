using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    public class IdleNode : Node
    {

        private float idleTime;
        private AbilityTimer<int, int> idleTimer;
        private GameObject boss;

        public IdleNode(GameObject boss, float idleTime) {
            this.idleTimer = new AbilityTimer<int, int>(idleTime, AfterTimer);
            this.idleTime= idleTime;
            this.boss = boss;
        }

        public override NodeState Evaluate()
        {
            if (state == NodeState.READY)
            {
                idleTimer.Restart();
                state = NodeState.RUNNING;
                boss.GetComponent<Animator>().SetBool("isJumping", false);
                boss.GetComponent<Animator>().SetBool("isAttacking", false);
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