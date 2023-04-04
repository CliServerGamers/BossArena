using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Scripts.Game.Boss.Decorators
{
    class TargetSelectionNode: BehaviorTree.Node
    {
        
        public TargetSelectionNode() { }

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

            // TODO: code here



            // when done, set state to success
            return NodeState.SUCCESS;
        }

    }
}
