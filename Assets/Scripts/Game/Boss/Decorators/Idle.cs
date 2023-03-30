using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BossArena.game
{
    public class Idle : Node
    {
        public Idle() { 
        
        }

        public NodeState Evaluate()
        {




            return NodeState.RUNNING;
        }

    }

}