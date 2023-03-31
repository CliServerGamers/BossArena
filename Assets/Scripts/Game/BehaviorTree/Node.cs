using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Game.BehaviorTree
{
    public enum NodeState
    {
        READY,
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected AbilityBase ability;
        public NodeState state;
        public Node parent;
        protected List<Node> children;
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            children = new List<Node>();
            parent = null;
            this.state = NodeState.READY;
        }

        public Node(List<Node> children) {
            this.children = new List<Node>();
            foreach (Node child in children)
            {
                _Attach(child);
            }
            this.state = NodeState.READY;
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
            this.state = NodeState.READY;
        }

        public virtual NodeState Evaluate() {
            return NodeState.READY;
        }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
            {
                return value;
            }
            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }

}
