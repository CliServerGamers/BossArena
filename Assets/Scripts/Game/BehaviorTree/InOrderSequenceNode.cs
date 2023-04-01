using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.BehaviorTree
{
    // like an AND gate: only if all child nodes succeed will it succeed itself
    public class InOrderSequenceNode : Node
    {
        public InOrderSequenceNode() : base() { }
        public InOrderSequenceNode(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        ResetAllChildNodeStates();
                        return state;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        continue;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return NodeState.RUNNING;
                }
            }

            // if all children are succeeded, put them back in the ready state
            if (children.All(node => node.state == NodeState.SUCCESS))
            {
                ResetAllChildNodeStates();
                state = NodeState.SUCCESS;
                return state;
            } 

            return state;
        }

        private void ResetAllChildNodeStates()
        {
            foreach (Node node in children)
            {
                node.state = NodeState.READY;
            }
        }

    }
}
